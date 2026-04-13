using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.EntityFrameworkCore;
using CursoSK.BankingBot.Data;

namespace CursoSK.BankingBot.Plugins;

/// <summary>Plugin para el proceso de onboarding bancario y gestión de préstamos.</summary>
public class OnboardingPlugin
{
    private readonly IServiceProvider _serviceProvider;

    public OnboardingPlugin(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [KernelFunction("consultar_requisitos_prestamo")]
    [Description("Consulta los requisitos necesarios para solicitar un tipo de préstamo específico")]
    public string ConsultarRequisitos([Description("Tipo de préstamo: personal, hipotecario, vehicular, empresarial")] string tipoPrestamo)
    {
        var requisitos = tipoPrestamo.ToLower() switch
        {
            "personal" => new[]
            {
                "1. Documento de Identidad (DPI) vigente",
                "2. Constancia laboral emitida en los últimos 30 días",
                "3. Últimos 3 estados de cuenta bancarios",
                "4. Recibo de servicios públicos reciente",
                "5. Dos referencias personales con teléfono",
                "6. Ingreso mínimo: L.15,000 mensuales",
                "📌 Tasa de interés: desde 16% hasta 22% anual",
                "📌 Plazo máximo: 60 meses",
                "📌 Monto máximo: L.500,000"
            },
            "hipotecario" => new[]
            {
                "1. Documento de Identidad (DPI) vigente",
                "2. Constancia de ingresos (últimos 2 años)",
                "3. Avalúo de la propiedad por perito autorizado",
                "4. Escritura pública del inmueble",
                "5. Póliza de seguro contra incendio y terremoto",
                "6. Estados financieros (si aplica)",
                "7. RTN actualizado",
                "📌 Tasa de interés: desde 8% hasta 12% anual",
                "📌 Plazo máximo: 240 meses (20 años)",
                "📌 Financiamiento: hasta 80% del valor del inmueble",
                "📌 Relación cuota/ingreso máxima: 30%"
            },
            "vehicular" => new[]
            {
                "1. Documento de Identidad (DPI) vigente",
                "2. Constancia laboral reciente",
                "3. Últimos 3 estados de cuenta",
                "4. Factura proforma del vehículo",
                "5. Prima mínima: 20% del valor del vehículo",
                "📌 Tasa de interés: desde 12% hasta 18% anual",
                "📌 Plazo máximo: 72 meses",
                "📌 Solo vehículos con antigüedad máxima de 3 años"
            },
            "empresarial" => new[]
            {
                "1. RTN de la empresa",
                "2. Escritura de constitución de la sociedad",
                "3. Estados financieros auditados (últimos 2 años)",
                "4. Plan de negocios / plan de inversión",
                "5. Flujo de caja proyectado",
                "6. Garantías (hipotecaria, prendaria o fiduciaria)",
                "7. DPI del representante legal",
                "📌 Tasa de interés: desde 10% hasta 16% anual",
                "📌 Plazo máximo: 120 meses",
                "📌 Monto: según capacidad de pago y garantías"
            },
            _ => new[] { "Tipo de préstamo no reconocido. Opciones: personal, hipotecario, vehicular, empresarial." }
        };

        return $"📋 Requisitos para préstamo {tipoPrestamo}:\n\n{string.Join("\n", requisitos)}";
    }

    [KernelFunction("simular_prestamo")]
    [Description("Calcula la cuota mensual estimada de un préstamo")]
    public string SimularPrestamo(
        [Description("Monto del préstamo en Lempiras")] decimal monto,
        [Description("Plazo en meses")] int plazoMeses,
        [Description("Tasa de interés anual (ejemplo: 18.5)")] decimal tasaAnual)
    {
        var tasaMensual = tasaAnual / 100 / 12;
        var cuota = monto * tasaMensual * (decimal)Math.Pow((double)(1 + tasaMensual), plazoMeses)
                    / ((decimal)Math.Pow((double)(1 + tasaMensual), plazoMeses) - 1);
        var totalPagar = cuota * plazoMeses;
        var totalIntereses = totalPagar - monto;

        return $"""
            💰 Simulación de Préstamo:
            ─────────────────────────
            Monto: L.{monto:N2}
            Plazo: {plazoMeses} meses ({plazoMeses / 12} años y {plazoMeses % 12} meses)
            Tasa anual: {tasaAnual}%
            Cuota mensual: L.{cuota:N2}
            Total a pagar: L.{totalPagar:N2}
            Total intereses: L.{totalIntereses:N2}
            """;
    }

    [KernelFunction("verificar_elegibilidad")]
    [Description("Verifica si un cliente es elegible para un préstamo basado en su ingreso y el monto solicitado")]
    public string VerificarElegibilidad(
        [Description("Ingreso mensual del cliente en Lempiras")] decimal ingresoMensual,
        [Description("Cuota mensual estimada del préstamo")] decimal cuotaMensual,
        [Description("Tipo de préstamo: personal, hipotecario, vehicular, empresarial")] string tipoPrestamo)
    {
        var relacionCuotaIngreso = (cuotaMensual / ingresoMensual) * 100;
        var limitePermitido = tipoPrestamo.ToLower() == "hipotecario" ? 30m : 40m;
        var norma = tipoPrestamo.ToLower() == "hipotecario"
            ? "Art. 15, Normas de Gestión de Riesgo Crediticio (Res. CNBS-GES-041-2019)"
            : "Art. 12, Normas de Gestión de Riesgo Crediticio (Res. CNBS-GES-041-2019)";

        var elegible = relacionCuotaIngreso <= limitePermitido;

        return $"""
            📊 Análisis de Elegibilidad:
            ─────────────────────────────
            Ingreso mensual: L.{ingresoMensual:N2}
            Cuota mensual: L.{cuotaMensual:N2}
            Relación cuota/ingreso: {relacionCuotaIngreso:N1}%
            Límite permitido: {limitePermitido}% ({norma})
            
            Resultado: {(elegible ? "✅ ELEGIBLE — Cumple con la relación cuota/ingreso" : $"❌ NO ELEGIBLE — Excede el límite del {limitePermitido}%. Opciones: reducir monto o ampliar plazo.")}
            """;
    }

    [KernelFunction("estado_solicitud")]
    [Description("Consulta el estado actual de una solicitud de préstamo por número de solicitud o identidad del cliente")]
    public async Task<string> EstadoSolicitud(
        [Description("Número de solicitud (ej: PREST-2026-001) o identidad del cliente")] string busqueda)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        var solicitud = await db.SolicitudesPrestamo
            .Include(s => s.Cliente)
            .Include(s => s.Requisitos)
            .FirstOrDefaultAsync(s => s.NumeroSolicitud == busqueda || s.Cliente!.Identidad == busqueda);

        if (solicitud == null)
            return $"No se encontró solicitud con referencia '{busqueda}'.";

        var reqCumplidos = solicitud.Requisitos.Count(r => r.Cumplido);
        var reqTotal = solicitud.Requisitos.Count;
        var reqPendientes = solicitud.Requisitos.Where(r => !r.Cumplido).Select(r => r.Nombre);

        return $"""
            📄 Solicitud: {solicitud.NumeroSolicitud}
            ─────────────────────────────
            Cliente: {solicitud.Cliente?.NombreCompleto}
            Tipo: Préstamo {solicitud.TipoPrestamo}
            Monto: L.{solicitud.MontoSolicitado:N2}
            Plazo: {solicitud.PlazoMeses} meses
            Estado: {solicitud.Estado.ToUpper().Replace("_", " ")}
            Riesgo: {solicitud.NivelRiesgo} (puntaje: {solicitud.PuntajeRiesgo}/100)
            Requisitos: {reqCumplidos}/{reqTotal} cumplidos
            {(reqPendientes.Any() ? $"📋 Pendientes:\n{string.Join("\n", reqPendientes.Select(r => $"  ❌ {r}"))}" : "✅ Todos los requisitos cumplidos")}
            """;
    }

    [KernelFunction("listar_tipos_prestamo")]
    [Description("Lista los tipos de préstamo disponibles con sus características principales")]
    public string ListarTiposPrestamo()
    {
        return """
            🏦 Tipos de Préstamo Disponibles:
            ──────────────────────────────────
            
            1. 💳 PERSONAL
               Monto: L.10,000 - L.500,000
               Plazo: 12-60 meses | Tasa: 16-22%
               Uso: Libre (viajes, educación, gastos médicos)
            
            2. 🏠 HIPOTECARIO
               Monto: L.500,000 - L.10,000,000
               Plazo: 60-240 meses | Tasa: 8-12%
               Uso: Compra o construcción de vivienda
            
            3. 🚗 VEHICULAR
               Monto: L.200,000 - L.2,000,000
               Plazo: 12-72 meses | Tasa: 12-18%
               Uso: Compra de vehículo nuevo o seminuevo
            
            4. 🏢 EMPRESARIAL
               Monto: Según capacidad de pago
               Plazo: 12-120 meses | Tasa: 10-16%
               Uso: Capital de trabajo, inversión, expansión
            """;
    }
}
