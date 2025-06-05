using Google.Cloud.Firestore;

namespace FirebaseApi.Models
{
    [FirestoreData]
    public class Pedido
    {
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty("tipoPollo")]
        public string TipoPollo { get; set; } = string.Empty;

        [FirestoreProperty("acompañamientos")]
        public string Acompañamientos { get; set; } = string.Empty;

        [FirestoreProperty("cantidad")]
        public int Cantidad { get; set; }

        [FirestoreProperty("precioTotal")]
        public double PrecioTotal { get; set; }

        [FirestoreProperty("UsuarioEmail")]
        public string UsuarioEmail { get; set; } = string.Empty;

        [FirestoreProperty("detalle")]
        public string Detalle { get; set; } = string.Empty;
    }
}
