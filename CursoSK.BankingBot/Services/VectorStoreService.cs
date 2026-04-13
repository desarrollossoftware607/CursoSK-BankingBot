using System.Collections.Concurrent;
using System.Numerics.Tensors;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using CursoSK.BankingBot.Models;

namespace CursoSK.BankingBot.Services;

/// <summary>Servicio de búsqueda vectorial y RAG para documentos legales/bancarios.
/// Implementación InMemory propia con similaridad coseno.</summary>
public class VectorStoreService
{
    private readonly Kernel _kernel;
    private readonly ConcurrentDictionary<string, DocumentoVectorial> _store = new();

    public VectorStoreService(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task IndexarDocumento(string titulo, string contenido, string categoria, string? fuente)
    {
        #pragma warning disable SKEXP0001, SKEXP0010
        var embeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        var embedding = await embeddingService.GenerateEmbeddingAsync($"{titulo}\n{contenido}");
        var doc = new DocumentoVectorial
        {
            Id = Guid.NewGuid().ToString(),
            Titulo = titulo,
            Contenido = contenido,
            Categoria = categoria,
            Fuente = fuente,
            Embedding = embedding
        };
        _store[doc.Id] = doc;
        #pragma warning restore SKEXP0001, SKEXP0010
    }

    public async Task<List<(DocumentoVectorial Doc, double? Score)>> BuscarSimilares(string consulta, int top = 3)
    {
        #pragma warning disable SKEXP0001, SKEXP0010
        var embeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        var queryEmbedding = await embeddingService.GenerateEmbeddingAsync(consulta);

        var results = _store.Values
            .Select(doc => (Doc: doc, Score: CosineSimilarity(queryEmbedding, doc.Embedding)))
            .OrderByDescending(x => x.Score)
            .Take(top)
            .Select(x => (x.Doc, (double?)x.Score))
            .ToList();

        return results;
        #pragma warning restore SKEXP0001, SKEXP0010
    }

    public int Count => _store.Count;

    private static double CosineSimilarity(ReadOnlyMemory<float> a, ReadOnlyMemory<float>? b)
    {
        if (b == null) return 0;
        return TensorPrimitives.CosineSimilarity(a.Span, b.Value.Span);
    }
}
