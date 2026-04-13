using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CursoSK.BankingBot.Data;
using CursoSK.BankingBot.DTOs;

namespace CursoSK.BankingBot.Controllers;

/// <summary>Gestión de clientes y solicitudes de préstamo (CRUD).</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("📋 Préstamos")]
public class PrestamosController : ControllerBase
{
    private readonly BankingDbContext _db;

    public PrestamosController(BankingDbContext db) => _db = db;

    /// <summary>Lista todas las solicitudes de préstamo con filtros opcionales.</summary>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? estado, [FromQuery] string? tipo)
    {
        var query = _db.SolicitudesPrestamo.Include(s => s.Cliente).AsQueryable();

        if (!string.IsNullOrEmpty(estado))
            query = query.Where(s => s.Estado == estado);
        if (!string.IsNullOrEmpty(tipo))
            query = query.Where(s => s.TipoPrestamo == tipo);

        var solicitudes = await query.OrderByDescending(s => s.FechaSolicitud)
            .Select(s => new
            {
                s.NumeroSolicitud,
                cliente = s.Cliente!.NombreCompleto,
                s.TipoPrestamo,
                s.MontoSolicitado,
                s.PlazoMeses,
                s.Estado,
                s.NivelRiesgo,
                s.FechaSolicitud
            })
            .ToListAsync();

        return Ok(solicitudes);
    }

    /// <summary>Obtiene el detalle completo de una solicitud con requisitos.</summary>
    [HttpGet("{numeroSolicitud}")]
    public async Task<IActionResult> Detalle(string numeroSolicitud)
    {
        var solicitud = await _db.SolicitudesPrestamo
            .Include(s => s.Cliente)
            .Include(s => s.Requisitos)
            .Include(s => s.HistorialEstados.OrderBy(h => h.Fecha))
            .FirstOrDefaultAsync(s => s.NumeroSolicitud == numeroSolicitud);

        if (solicitud == null) return NotFound();

        return Ok(new
        {
            solicitud.NumeroSolicitud,
            cliente = new { solicitud.Cliente!.NombreCompleto, solicitud.Cliente.Identidad, solicitud.Cliente.IngresoMensual },
            solicitud.TipoPrestamo,
            solicitud.MontoSolicitado,
            solicitud.PlazoMeses,
            solicitud.TasaInteres,
            solicitud.CuotaMensualEstimada,
            solicitud.Estado,
            solicitud.PuntajeRiesgo,
            solicitud.NivelRiesgo,
            requisitos = solicitud.Requisitos.Select(r => new { r.Nombre, r.Obligatorio, r.Cumplido, r.Observacion }),
            historial = solicitud.HistorialEstados.Select(h => new { h.EstadoAnterior, h.EstadoNuevo, h.Observacion, h.Fecha })
        });
    }

    /// <summary>Lista todos los clientes registrados.</summary>
    [HttpGet("clientes")]
    public async Task<IActionResult> ListarClientes()
    {
        var clientes = await _db.Clientes
            .Select(c => new
            {
                c.Identidad,
                c.NombreCompleto,
                c.Ocupacion,
                c.IngresoMensual,
                c.Estado,
                totalSolicitudes = c.Solicitudes.Count
            })
            .ToListAsync();
        return Ok(clientes);
    }

    /// <summary>Lista las leyes y normativas vigentes.</summary>
    [HttpGet("normativas")]
    public async Task<IActionResult> ListarNormativas([FromQuery] string? categoria)
    {
        var query = _db.LeyesNormativas.Where(l => l.Vigente).AsQueryable();
        if (!string.IsNullOrEmpty(categoria))
            query = query.Where(l => l.Categoria == categoria);

        var leyes = await query.Select(l => new
        {
            l.Titulo,
            l.NumeroLey,
            l.Categoria,
            l.FechaPublicacion,
            totalArticulos = l.Id // simple count proxy
        }).ToListAsync();

        return Ok(leyes);
    }
}
