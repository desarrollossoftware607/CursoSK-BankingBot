# Paso a Paso — Sesión 9: RAG Completo + Agent Framework

> **Rama Git:** `sesion/09`

---

## Archivos a crear/modificar esta sesión

| Archivo | Acción |
|---|---|
| `Controllers/RAGController.cs` | **Crear** |
| `DTOs/Requests.cs` | Modificar (agregar DTOs de RAG) |

---

## 1. Controllers/RAGController.cs

```csharp
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

    [HttpPost("indexar")]
    public async Task<IActionResult> Indexar([FromBody] IndexarDocumentoRequest request)
    {
        var id = await _vectorStore.IndexarDocumento(request.Titulo, request.Contenido, request.Categoria);
        return Ok(new { id, mensaje = "Documento indexado", totalDocumentos = _vectorStore.Count });
    }

    [HttpPost("buscar")]
    public async Task<IActionResult> Buscar([FromBody] BusquedaSemanticaRequest request)
    {
        var resultados = await _vectorStore.BuscarSimilares(request.Consulta, request.Top);
        return Ok(resultados.Select(r => new
        {
            titulo = r.Doc.Titulo,
            categoria = r.Doc.Categoria,
            score = r.Score,
            contenido = r.Doc.Contenido.Length > 200
                ? r.Doc.Contenido[..200] + "..."
                : r.Doc.Contenido
        }));
    }

    [HttpPost("consultar")]
    public async Task<IActionResult> ConsultarRAG([FromBody] BusquedaSemanticaRequest request)
    {
        var resultados = await _vectorStore.BuscarSimilares(request.Consulta, request.Top);
        var contexto = string.Join("\n\n---\n\n",
            resultados.Select(r => $"[{r.Doc.Titulo}] (Score: {r.Score:F3})\n{r.Doc.Contenido}"));

        var prompt = $"""
            Responde ÚNICAMENTE basándote en el siguiente contexto.
            Si no hay información suficiente, di "No tengo información suficiente."

            CONTEXTO:
            {contexto}

            PREGUNTA: {request.Consulta}
            """;
        var result = await _kernel.InvokePromptAsync(prompt);
        return Ok(new { respuesta = result.ToString(), fragmentosUsados = resultados.Count });
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedFAQs()
    {
        var faqs = new[]
        {
            ("¿Qué es Semantic Kernel?", "Semantic Kernel es un SDK open-source de Microsoft que permite integrar modelos de lenguaje (LLMs) en aplicaciones C#, Python y Java. Actúa como orquestador central coordinando IA, plugins y memoria.", "SK"),
            ("¿Qué es RAG?", "RAG (Retrieval-Augmented Generation) es un patrón que combina búsqueda vectorial con generación de texto. Primero recupera fragmentos relevantes de una base de conocimiento, luego los inyecta como contexto del prompt para que el LLM responda con información fundamentada.", "RAG"),
            ("¿Qué es un embedding?", "Un embedding es una representación numérica (vector) del significado de un texto. Permite medir la similaridad semántica entre textos usando métricas como la similaridad coseno.", "Embeddings"),
        };

        foreach (var (titulo, contenido, categoria) in faqs)
            await _vectorStore.IndexarDocumento(titulo, contenido, categoria);

        return Ok(new { mensaje = $"{faqs.Length} FAQs indexados", total = _vectorStore.Count });
    }
}
```

---

## 2. Agregar DTOs a DTOs/Requests.cs

Agregar al final del archivo:

```csharp
public record IndexarDocumentoRequest(string Titulo, string Contenido, string Categoria);
public record BusquedaSemanticaRequest(string Consulta, int Top = 3);
```

---

## 3. Probar

```powershell
dotnet run
```

1. `POST /api/rag/seed` → Cargar FAQs de ejemplo
2. `POST /api/rag/buscar` → `{ "consulta": "¿Qué es Semantic Kernel?", "top": 3 }`
3. `POST /api/rag/consultar` → `{ "consulta": "¿Qué es RAG y para qué sirve?" }`

---

## 4. Azure Setup (opcional)

Para usar Azure AI Search en vez del VectorStore en memoria:

```powershell
cd Scripts/Azure
.\09-crear-ai-search.ps1
```
