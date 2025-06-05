using FirebaseApi.Models;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;

namespace FirebaseApi.Services;

public class FirebaseService
{
    private readonly FirestoreDb _db;
    private readonly ILogger<FirebaseService> _logger;

    public FirebaseService(IConfiguration config, ILogger<FirebaseService> logger)
    {
        _logger = logger;

        var projectId = config["Firebase:ProjectId"];
        var credPath = config["Firebase:CredentialPath"] ?? "/etc/secrets/Clave.json";

        if (string.IsNullOrWhiteSpace(projectId))
        {
            throw new ArgumentException("ProjectId de Firebase no configurado.");
        }

        if (!File.Exists(credPath))
        {
            throw new FileNotFoundException($"No se encontró el archivo de credenciales en: {credPath}");
        }

        // Leer contenido original del archivo
        var contenidoOriginal = File.ReadAllText(credPath);

        // Revisar si contiene \n literales y reemplazarlos por saltos de línea reales
        var contenidoCorregido = contenidoOriginal.Replace("\\n", "\n");

        // Guardar contenido corregido en un archivo temporal
        var tempPath = "/tmp/firebase_key_corrected.json";
        File.WriteAllText(tempPath, contenidoCorregido);

        // Loguear contenido parcial para debug
        _logger.LogInformation("Contenido parcial credenciales corregidas: {Contenido}",
            contenidoCorregido.Length > 100 ? contenidoCorregido.Substring(0, 100) + "..." : contenidoCorregido);

        // Establecer variable de entorno para la ruta del archivo corregido
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);

        // Crear instancia FirestoreDb con projectId
        _db = FirestoreDb.Create(projectId);
    }

    public async Task<List<Pedido>> ObtenerPedidosPorUsuario(string email)
    {
        var pedidos = new List<Pedido>();

        try
        {
            var snapshot = await _db.Collection("pedidos")
                .WhereEqualTo("UsuarioEmail", email)
                .GetSnapshotAsync();

            foreach (var doc in snapshot.Documents)
            {
                pedidos.Add(doc.ConvertTo<Pedido>());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedidos.");
        }

        return pedidos;
    }
}
