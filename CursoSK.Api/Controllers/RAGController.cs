using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using CursoSK.Api.DTOs;
using CursoSK.Api.Services;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("9️⃣ RAG — Sesiones 8-9")]
public class RAGController : ControllerBase
{
    private readonly VectorStoreService _vectorStore;
    private readonly Kernel _kernel;

    public RAGController(VectorStoreService vectorStore, Kernel kernel)
    {
        _vectorStore = vectorStore;
        _kernel = kernel;
    }

    /// <summary>Indexa un documento en el vector store (genera embedding).</summary>
    [HttpPost("indexar")]
    public async Task<IActionResult> Indexar([FromBody] IndexarDocumentoRequest request)
    {
        var id = await _vectorStore.IndexarDocumento(request.Titulo, request.Contenido, request.Categoria, request.Fuente);
        return Ok(new { id, mensaje = "Documento indexado", totalDocumentos = _vectorStore.Count });
    }

    /// <summary>Búsqueda semántica (solo vector search, sin LLM).</summary>
    [HttpPost("buscar")]
    public async Task<IActionResult> Buscar([FromBody] BusquedaSemanticaRequest request)
    {
        var resultados = await _vectorStore.BuscarSimilares(request.Consulta, request.Top);
        return Ok(resultados.Select(r => new
        {
            titulo = r.Doc.Titulo,
            contenido = r.Doc.Contenido[..Math.Min(300, r.Doc.Contenido.Length)],
            categoria = r.Doc.Categoria,
            similitud = r.Score
        }));
    }

    /// <summary>Consulta RAG: vector search + LLM genera respuesta con contexto.</summary>
    [HttpPost("consultar")]
    public async Task<IActionResult> ConsultarRAG([FromBody] BusquedaSemanticaRequest request)
    {
        var resultados = await _vectorStore.BuscarSimilares(request.Consulta, request.Top);

        if (resultados.Count == 0)
            return Ok(new { respuesta = "No se encontraron documentos relevantes.", fuentes = Array.Empty<string>() });

        var contexto = string.Join("\n\n---\n\n",
            resultados.Select(r => $"[{r.Doc.Titulo}] (similitud: {r.Score:F3})\n{r.Doc.Contenido}"));

        var prompt = $"""
            Eres un asistente experto. Responde la pregunta del usuario usando SOLO la información del contexto proporcionado.
            Si la información no está en el contexto, indica que no tienes suficiente información.
            Cita las fuentes cuando sea posible.

            ## Contexto:
            {contexto}

            ## Pregunta:
            {request.Consulta}

            ## Respuesta:
            """;

        var result = await _kernel.InvokePromptAsync(prompt);
        return Ok(new
        {
            respuesta = result.ToString(),
            fuentes = resultados.Select(r => new { r.Doc.Titulo, r.Score }),
            documentosEnStore = _vectorStore.Count
        });
    }

    /// <summary>Carga FAQs de ejemplo para demostración de RAG.</summary>
    [HttpPost("seed")]
    public async Task<IActionResult> SeedFAQs()
    {
        var faqs = new[]
        {
            ("¿Qué es Semantic Kernel?", "Semantic Kernel es un SDK open-source de Microsoft que permite integrar modelos de lenguaje (LLMs) como GPT en aplicaciones empresariales. Soporta C#, Python y Java. Actúa como orquestador entre la lógica de negocio y los servicios de IA.", "faq"),
            ("¿Qué es un Plugin en SK?", "Un Plugin es una clase C# con métodos decorados con [KernelFunction] y [Description]. El LLM puede invocar estas funciones automáticamente via Function Calling cuando determina que son relevantes para la consulta del usuario.", "faq"),
            ("¿Qué es RAG?", "RAG (Retrieval-Augmented Generation) es un patrón donde primero se buscan documentos relevantes en un vector store usando embeddings, y luego se inyecta ese contexto en el prompt del LLM para generar respuestas fundamentadas en datos reales.", "faq"),
            ("¿Qué es un embedding?", "Un embedding es una representación vectorial (array de floats) de un texto. Textos semánticamente similares producen vectores cercanos en el espacio. Se generan con modelos como text-embedding-3-small y se usan para búsqueda semántica.", "faq"),
            ("¿Qué es Function Calling?", "Function Calling es la capacidad del LLM de analizar el prompt del usuario, determinar si necesita invocar una función externa (plugin), generar los argumentos correctos, ejecutar la función, y usar el resultado para formular la respuesta.", "faq")
        };

        foreach (var (titulo, contenido, cat) in faqs)
            await _vectorStore.IndexarDocumento(titulo, contenido, cat);

        return Ok(new { mensaje = $"Se cargaron {faqs.Length} FAQs en el vector store", total = _vectorStore.Count });
    }
}
