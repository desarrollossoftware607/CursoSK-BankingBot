using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.EntityFrameworkCore;
using CursoSK.BankingBot.Data;

namespace CursoSK.BankingBot.Plugins;

/// <summary>Plugin para consultas legales y regulatorias del sector bancario.</summary>
public class LegalPlugin
{
    private readonly IServiceProvider _serviceProvider;

    public LegalPlugin(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [KernelFunction("buscar_normativa")]
    [Description("Busca leyes o normativas bancarias por categoría o tema")]
    public async Task<string> BuscarNormativa(
        [Description("Categoría: bancaria, lavado_activos, proteccion_consumidor, tributaria. O un término de búsqueda libre.")] string busqueda)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        var leyes = await db.LeyesNormativas
            .Where(l => l.Vigente &&
                (l.Categoria.Contains(busqueda) ||
                 l.Titulo.Contains(busqueda) ||
                 l.Contenido.Contains(busqueda)))
            .ToListAsync();

        if (!leyes.Any())
            return $"No se encontraron normativas relacionadas con '{busqueda}'.";

        var resultado = leyes.Select(l =>
            $"📜 {l.Titulo} ({l.NumeroLey})\nCategoría: {l.Categoria}\n{l.Contenido}\n");

        return $"Encontradas {leyes.Count} normativa(s):\n\n{string.Join("\n───────────────\n", resultado)}";
    }

    [KernelFunction("consultar_articulo")]
    [Description("Consulta un artículo específico de una ley o normativa")]
    public async Task<string> ConsultarArticulo(
        [Description("Número de artículo, ej: 'Art. 12'")] string numeroArticulo)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        var articulos = await db.ArticulosLey
            .Include(a => a.Ley)
            .Where(a => a.NumeroArticulo.Contains(numeroArticulo))
            .ToListAsync();

        if (!articulos.Any())
            return $"No se encontró el artículo '{numeroArticulo}'.";

        var resultado = articulos.Select(a =>
            $"📑 {a.NumeroArticulo} — {a.Ley?.Titulo} ({a.Ley?.NumeroLey})\n{a.Texto}\n📝 Resumen: {a.Resumen}");

        return string.Join("\n\n", resultado);
    }

    [KernelFunction("verificar_cumplimiento_kyc")]
    [Description("Verifica si un cliente cumple con los requisitos KYC (Know Your Customer) según la normativa vigente")]
    public string VerificarKYC(
        [Description("Tiene documento de identidad válido")] bool tieneIdentidad,
        [Description("Tiene comprobante de domicilio")] bool tieneComprobanteDomicilio,
        [Description("Tiene constancia de ingresos")] bool tieneConstanciaIngresos,
        [Description("Monto de la transacción en USD")] decimal montoUSD)
    {
        var items = new List<string>();
        var cumple = true;

        if (tieneIdentidad)
            items.Add("✅ Identificación del cliente — Cumple");
        else { items.Add("❌ Identificación del cliente — FALTA"); cumple = false; }

        if (tieneComprobanteDomicilio)
            items.Add("✅ Comprobante de domicilio — Cumple");
        else { items.Add("❌ Comprobante de domicilio — FALTA"); cumple = false; }

        if (tieneConstanciaIngresos)
            items.Add("✅ Constancia de ingresos — Cumple");
        else { items.Add("❌ Constancia de ingresos — FALTA"); cumple = false; }

        if (montoUSD >= 10000)
        {
            items.Add($"⚠️ Transacción de US${montoUSD:N2} — Requiere reporte a la UIF (Art. 12, Decreto 144-2014)");
            items.Add("⚠️ Se debe completar formulario de Debida Diligencia Reforzada");
        }

        return $"""
            🔍 Verificación KYC (Know Your Customer):
            ────────────────────────────────────────────
            {string.Join("\n", items)}
            
            Base legal: Decreto 144-2014, Ley Contra el Lavado de Activos
            Resultado general: {(cumple ? "✅ CUMPLE con KYC básico" : "❌ NO CUMPLE — Documentación incompleta")}
            """;
    }

    [KernelFunction("evaluar_riesgo_legal")]
    [Description("Evalúa el riesgo legal de una operación crediticia")]
    public string EvaluarRiesgoLegal(
        [Description("Monto del préstamo en Lempiras")] decimal monto,
        [Description("Tiene garantías documentadas")] bool tieneGarantias,
        [Description("Cliente tiene historial crediticio")] bool tieneHistorial,
        [Description("Documentación completa")] bool documentacionCompleta)
    {
        var puntaje = 0;
        var observaciones = new List<string>();

        if (!tieneGarantias) { puntaje += 30; observaciones.Add("⚠️ Sin garantías documentadas — Riesgo incrementado"); }
        if (!tieneHistorial) { puntaje += 25; observaciones.Add("⚠️ Sin historial crediticio — Cliente nuevo"); }
        if (!documentacionCompleta) { puntaje += 25; observaciones.Add("❌ Documentación incompleta"); }
        if (monto > 500000) { puntaje += 10; observaciones.Add("📌 Monto alto — Requiere autorización de comité"); }
        if (monto > 1000000) { puntaje += 10; observaciones.Add("📌 Monto > L.1,000,000 — Requiere estados financieros auditados (Art. 20, Decreto 129-2004)"); }

        var nivel = puntaje switch
        {
            <= 20 => "BAJO",
            <= 40 => "MEDIO",
            <= 60 => "ALTO",
            _ => "MUY ALTO"
        };

        return $"""
            ⚖️ Evaluación de Riesgo Legal:
            ────────────────────────────────
            Puntaje de riesgo: {puntaje}/100
            Nivel: {nivel}
            
            Observaciones:
            {string.Join("\n", observaciones)}
            
            Clasificación crediticia sugerida: {(puntaje <= 20 ? "A (Normal)" : puntaje <= 40 ? "B (Potencial)" : puntaje <= 60 ? "C (Deficiente)" : "D (Dudosa)")}
            Ref: Art. 5, Resolución CNBS-GES-041-2019
            """;
    }
}
