using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace CursoSK.BankingBot.Plugins;

/// <summary>Plugin para cálculos financieros bancarios.</summary>
public class CalculadoraFinancieraPlugin
{
    [KernelFunction("calcular_cuota")]
    [Description("Calcula la cuota mensual de un préstamo usando el sistema francés (cuota fija)")]
    public string CalcularCuota(
        [Description("Monto del préstamo en Lempiras")] decimal monto,
        [Description("Plazo en meses")] int plazoMeses,
        [Description("Tasa de interés anual")] decimal tasaAnual)
    {
        var tasaMensual = tasaAnual / 100 / 12;
        var cuota = monto * tasaMensual * (decimal)Math.Pow((double)(1 + tasaMensual), plazoMeses)
                    / ((decimal)Math.Pow((double)(1 + tasaMensual), plazoMeses) - 1);
        return $"La cuota mensual sería de L.{cuota:N2}";
    }

    [KernelFunction("tabla_amortizacion")]
    [Description("Genera una tabla de amortización resumida (primeros 6 y últimos 3 meses)")]
    public string TablaAmortizacion(
        [Description("Monto del préstamo")] decimal monto,
        [Description("Plazo en meses")] int plazoMeses,
        [Description("Tasa anual")] decimal tasaAnual)
    {
        var tasaMensual = tasaAnual / 100 / 12;
        var cuota = monto * tasaMensual * (decimal)Math.Pow((double)(1 + tasaMensual), plazoMeses)
                    / ((decimal)Math.Pow((double)(1 + tasaMensual), plazoMeses) - 1);

        var saldo = monto;
        var lineas = new List<string> { "Mes | Cuota | Capital | Interés | Saldo", "──── | ──── | ──── | ──── | ────" };

        for (int i = 1; i <= plazoMeses; i++)
        {
            var interes = saldo * tasaMensual;
            var capital = cuota - interes;
            saldo -= capital;
            if (saldo < 0) saldo = 0;

            if (i <= 6 || i > plazoMeses - 3)
                lineas.Add($"{i,4} | L.{cuota:N2} | L.{capital:N2} | L.{interes:N2} | L.{saldo:N2}");
            else if (i == 7)
                lineas.Add("... | ... | ... | ... | ...");
        }

        return $"📊 Tabla de Amortización (Sistema Francés):\n{string.Join("\n", lineas)}";
    }

    [KernelFunction("convertir_moneda")]
    [Description("Convierte entre Lempiras (HNL) y Dólares (USD)")]
    public string ConvertirMoneda(
        [Description("Monto a convertir")] decimal monto,
        [Description("Moneda origen: HNL o USD")] string monedaOrigen)
    {
        // Tipo de cambio referencial BCH
        const decimal tipoCambioCompra = 24.65m;
        const decimal tipoCambioVenta = 24.95m;

        if (monedaOrigen.ToUpper() == "HNL")
        {
            var usd = monto / tipoCambioVenta;
            return $"L.{monto:N2} HNL = US${usd:N2} (TC venta: {tipoCambioVenta})";
        }
        else
        {
            var hnl = monto * tipoCambioCompra;
            return $"US${monto:N2} = L.{hnl:N2} HNL (TC compra: {tipoCambioCompra})";
        }
    }
}
