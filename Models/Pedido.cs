using System;

namespace FirebaseApi.Models
{
    public class Pedido
    {
        public string Id { get; set; } = string.Empty;
        public string TipoPollo { get; set; } = string.Empty;
        public string Acompañamientos { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public double PrecioTotal { get; set; }
        public string UsuarioEmail { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
    }
}
