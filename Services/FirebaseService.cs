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

        var credJson = config["Clave.json"];

        var projectId = config["Firebase__ProjectId"];

        if (string.IsNullOrWhiteSpace(credJson) || string.IsNullOrWhiteSpace(projectId))
        {
            throw new ArgumentException("Credenciales o ID de proyecto no válidos.");
        }

        var tempPath = Path.Combine(Path.GetTempPath(), "firebase-creds.json");
        File.WriteAllText(tempPath, credJson);

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);
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
