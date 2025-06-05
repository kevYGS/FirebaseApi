using Google.Cloud.Firestore;

namespace FirebaseApi.Models
{
    [FirestoreData]
    public class Pedido
    {
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty]
        public string tipoPollo { get; set; } = string.Empty;

        [FirestoreProperty]
        public string acompañamientos { get; set; } = string.Empty;

        [FirestoreProperty]
        public int cantidad { get; set; }

        [FirestoreProperty]
        public double precioTotal { get; set; }

        [FirestoreProperty]
        public string UsuarioEmail { get; set; } = string.Empty;

        [FirestoreProperty]
        public string detalle { get; set; } = string.Empty;
    }
}
