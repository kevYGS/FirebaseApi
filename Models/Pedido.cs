using Google.Cloud.Firestore;

namespace FirebaseApi.Models
{
    [FirestoreData]
    public class Pedido
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty]
        public string TipoPollo { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Acompañamientos { get; set; } = string.Empty;

        [FirestoreProperty]
        public int Cantidad { get; set; }

        [FirestoreProperty]
        public double PrecioTotal { get; set; }

        [FirestoreProperty]
        public string UsuarioEmail { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Detalle { get; set; } = string.Empty;
    }
}
