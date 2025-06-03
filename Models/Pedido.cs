
namespace FirebaseApi.Models;

public class Pedido
{
    public string TipoPollo { get; set; }
    public string Acompañamientos { get; set; }
    public int Cantidad { get; set; }
    public double PrecioTotal { get; set; }
    public string UsuarioEmail { get; set; }
}
