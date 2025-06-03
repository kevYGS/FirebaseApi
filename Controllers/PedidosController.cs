using FirebaseApi.Models;
using FirebaseApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FirebaseApi.Controllers;

public class ResumenPedidosDto
{
    public string Usuario { get; set; }
    public int TotalPedidos { get; set; }
    public int CantidadTotal { get; set; }
    public double TotalGanado { get; set; }
    public int Dias { get; set; }
    public int Personas { get; set; }
    public double RepartoPorPersona { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly FirebaseService _firebaseService;

    public PedidosController(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    [HttpGet("resumen")]
    public async Task<IActionResult> ObtenerResumen(
        [FromQuery, Required, EmailAddress] string email,
        [FromQuery] int dias = 1,
        [FromQuery] int personas = 1)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (dias <= 0)
            dias = 1;
        if (personas <= 0)
            personas = 1;

        try
        {
            var pedidos = await _firebaseService.ObtenerPedidosPorUsuario(email);

            if (pedidos == null || pedidos.Count == 0)
            {
                return NotFound(new { mensaje = "No se encontraron pedidos para este usuario." });
            }

            double totalGanado = pedidos.Sum(p => p.PrecioTotal) * dias;
            int totalCantidad = pedidos.Sum(p => p.Cantidad);
            double porPersona = totalGanado / personas;

            var resumen = new ResumenPedidosDto
            {
                Usuario = email,
                TotalPedidos = pedidos.Count,
                CantidadTotal = totalCantidad,
                TotalGanado = Math.Round(totalGanado, 2),
                Dias = dias,
                Personas = personas,
                RepartoPorPersona = Math.Round(porPersona, 2)
            };

            return Ok(resumen);
        }
        catch (Exception ex)
        {
           
            return StatusCode(500, new { mensaje = "Error interno en el servidor", detalle = ex.Message });
        }
    }
}
