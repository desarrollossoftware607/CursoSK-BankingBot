# Paso a Paso — Sesión 8: Intro Embeddings + VectorStoreService

> **Rama Git:** `sesion/08`

---

## Archivos a crear/modificar esta sesión

| Archivo | Acción |
|---|---|
| `Models/DocumentoVectorial.cs` | **Crear** |
| `Services/VectorStoreService.cs` | **Crear** |
| `Program.cs` | Modificar (registrar embeddings + VectorStoreService) |

---

## Instalar paquete NuGet adicional

```powershell
dotnet add package System.Numerics.Tensors --version 10.0.4
```

---

## 1. Models/DocumentoVectorial.cs

```csharp
namespace CursoSK.Api.Models;

public class DocumentoVectorial
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public ReadOnlyMemory<float>? Embedding { get; set; }
}
```

---

## 2. Services/VectorStoreService.cs

```csharp
using System.Collections.Concurrent;
using System.Numerics.Tensors;
using Microsoft.SemanticKernel.Embeddings;
using CursoSK.Api.Models;

namespace CursoSK.Api.Services;

#pragma warning disable SKEXP0010

public class VectorStoreService
{
    private readonly ConcurrentDictionary<string, DocumentoVectorial> _store = new();
    private readonly ITextEmbeddingGenerationService? _embeddingService;
    private readonly ILogger<VectorStoreService> _logger;

    public VectorStoreService(IServiceProvider sp, ILogger<VectorStoreService> logger)
    {
        _embeddingService = sp.GetService<ITextEmbeddingGenerationService>();
        _logger = logger;
    }

    public async Task<string> IndexarDocumento(string titulo, string contenido, string categoria)
    {
        if (_embeddingService == null)
            throw new InvalidOperationException("Servicio de embeddings no configurado.");

        var embedding = await _embeddingService.GenerateEmbeddingAsync(contenido);
        var doc = new DocumentoVectorial
        {
            Titulo = titulo,
            Contenido = contenido,
            Categoria = categoria,
            Embedding = embedding
        };
        _store.TryAdd(doc.Id, doc);
        _logger.LogInformation("Documento indexado: {Titulo} ({Id})", titulo, doc.Id);
        return doc.Id;
    }

    public async Task<List<(DocumentoVectorial Doc, double Score)>> BuscarSimilares(string consulta, int top = 3)
    {
        if (_embeddingService == null)
            throw new InvalidOperationException("Servicio de embeddings no configurado.");

        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(consulta);
        return _store.Values
            .Where(d => d.Embedding.HasValue)
            .Select(d => (Doc: d, Score: (double)TensorPrimitives.CosineSimilarity(
                queryEmbedding.Span, d.Embedding!.Value.Span)))
            .OrderByDescending(x => x.Score)
            .Take(top)
            .ToList();
    }

    public int Count => _store.Count;
}

#pragma warning restore SKEXP0010
```

---

## 3. Modificar Program.cs

Agregar al `kernelBuilder`:

```csharp
// Embeddings (Sesión 8)
#pragma warning disable SKEXP0010
kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
    deploymentName: builder.Configuration["LLMSettings:Embedding:DeploymentName"]!,
    endpoint: builder.Configuration["LLMSettings:Embedding:Endpoint"]!,
    apiKey: builder.Configuration["LLMSettings:Embedding:ApiKey"]!);
#pragma warning restore SKEXP0010
```

Agregar al DI:

```csharp
builder.Services.AddSingleton<VectorStoreService>();
```

No olvidar el `using`:

```csharp
using CursoSK.Api.Services;
```

---

## 4. Probar

Los endpoints de embeddings se probarán en la sesión 9 con el `RAGController`. Por ahora verificar que el proyecto compila:

```powershell
dotnet build
```
