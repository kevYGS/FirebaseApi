using FirebaseApi.Models;
using Google.Cloud.Firestore;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;

namespace FirebaseApi.Services
{
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

            // Arreglar contenido de credenciales para evitar problemas con saltos de línea
            var contenidoOriginal = File.ReadAllText(credPath);
            var contenidoCorregido = contenidoOriginal.Replace("\\n", "\n");
            var tempPath = "/tmp/firebase_key_corrected.json";
            File.WriteAllText(tempPath, contenidoCorregido);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);

            _db = FirestoreDb.Create(projectId);
        }

       
        public async Task<string> ObtenerUserIdPorEmail(string email)
        {
            try
            {
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
                return userRecord.Uid;
            }
            catch (FirebaseAuthException ex)
            {
                _logger.LogWarning(ex, $"No se encontró usuario con email {email}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener userId por email");
                throw;
            }
        }

        // Obtener pedidos para un usuario por su UID
        public async Task<List<Pedido>> ObtenerPedidosPorUsuario(string userId)
        {
            var pedidos = new List<Pedido>();

            try
            {
                var pedidosCollection = _db.Collection("usuarios").Document(userId).Collection("pedidos");
                var snapshot = await pedidosCollection.GetSnapshotAsync();

                foreach (var doc in snapshot.Documents)
                {
                    var pedido = doc.ConvertTo<Pedido>();
                    pedido.Id = doc.Id; // asignar el id del documento si lo necesitas
                    pedidos.Add(pedido);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener pedidos para el usuario {userId}");
            }

            return pedidos;
        }
    }
}
