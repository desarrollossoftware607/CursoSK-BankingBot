using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using CursoSK.BankingBot.DTOs;
using CursoSK.BankingBot.Services;

namespace CursoSK.BankingBot.Controllers;

/// <summary>Asistente legal bancario con RAG (Retrieval Augmented Generation).</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("⚖️ Asistente Legal")]
public class LegalController : ControllerBase
{
    private readonly Kernel _kernel;
    private readonly VectorStoreService _vectorStore;

    public LegalController(Kernel kernel, VectorStoreService vectorStore)
    {
        _kernel = kernel;
        _vectorStore = vectorStore;
    }

    /// <summary>
    /// Consulta legal con Function Calling — invoca el plugin legal automáticamente
    /// para buscar normativas, verificar KYC y evaluar riesgos.
    /// </summary>
    [HttpPost("consultar")]
    public async Task<IActionResult> Consultar([FromBody] ConsultaLegalRequest request)
    {
        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var systemPrompt = """
            Eres un asistente legal bancario experto en regulación financiera.
            - Cita siempre el número de ley, decreto o resolución aplicable.
            - Indica el artículo específico cuando sea posible.
            - Explica las implicaciones prácticas para el banco y el cliente.
            - Si hay riesgo de incumplimiento, indícalo claramente.
            - Usa lenguaje técnico pero comprensible.
            """;

        var prompt = $"{systemPrompt}\n\nConsulta: {request.Consulta}";
        var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));
        return Ok(new { respuesta = result.ToString() });
    }

    /// <summary>
    /// Consulta legal con RAG — busca primero en la base vectorial de leyes
    /// y usa el contexto encontrado para generar una respuesta fundamentada.
    /// </summary>
    [HttpPost("consultar/rag")]
    public async Task<IActionResult> ConsultarConRAG([FromBody] RagPreguntaRequest request)
    {
        // 1. Buscar documentos relevantes en el vector store
        var documentos = await _vectorStore.BuscarSimilares(request.Pregunta, 5);

        if (!documentos.Any())
            return Ok(new { respuesta = "No se encontraron documentos legales relevantes. Intente reformular su consulta.", fuentes = Array.Empty<object>() });

        // 2. Construir contexto con los documentos encontrados
        var contexto = string.Join("\n\n---\n\n", documentos.Select(d =>
            $"📜 {d.Doc.Titulo} [{d.Doc.Categoria}] (Relevancia: {d.Score:P0})\n{d.Doc.Contenido}"));

        // 3. Generar respuesta con RAG
        var prompt = $"""
            Eres un asistente legal bancario. Responde la siguiente consulta ÚNICAMENTE
            con base en los documentos legales proporcionados como contexto.
            Si la información no está en los documentos, indícalo.
            Cita la fuente (nombre de ley y artículo) en tu respuesta.

            === DOCUMENTOS LEGALES DE REFERENCIA ===
            {contexto}
            === FIN DE DOCUMENTOS ===

            Consulta del usuario: {request.Pregunta}
            """;

        var result = await _kernel.InvokePromptAsync(prompt);

        return Ok(new
        {
            respuesta = result.ToString(),
            fuentes = documentos.Select(d => new
            {
                titulo = d.Doc.Titulo,
                categoria = d.Doc.Categoria,
                relevancia = d.Score,
                fuente = d.Doc.Fuente
            })
        });
    }
}
