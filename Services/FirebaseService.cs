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

        var projectId = config["Firebase__ProjectId"];
        var credPath = "/etc/secrets/Clave.json"; 

        if (string.IsNullOrWhiteSpace(projectId))
        {
            throw new ArgumentException("ProjectId de Firebase no configurado.");
        }

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credPath);
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
