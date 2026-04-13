using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Plugins;

public class ClimaPlugin
{
    [KernelFunction("obtener_clima")]
    [Description("Obtiene el clima actual de una ciudad específica")]
    public string ObtenerClima(
        [Description("Nombre de la ciudad")] string ciudad)
    {
        var climas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "tegucigalpa", "☀️ 28°C, Soleado" },
            { "san pedro sula", "🌤️ 32°C, Parcialmente nublado" },
            { "la ceiba", "🌧️ 26°C, Lluvioso" },
            { "madrid", "🌤️ 22°C, Despejado" },
            { "ciudad de mexico", "⛅ 20°C, Nublado" },
            { "new york", "❄️ 5°C, Frío" }
        };
        return climas.GetValueOrDefault(ciudad, $"No tengo datos meteorológicos para {ciudad}");
    }

    [KernelFunction("obtener_fecha_hora")]
    [Description("Obtiene la fecha y hora actual del servidor")]
    public string ObtenerFechaHora()
        => $"Fecha: {DateTime.Now:dd/MM/yyyy}, Hora: {DateTime.Now:hh:mm tt}";
}
