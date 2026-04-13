using System.Collections.Concurrent;
using System.Numerics.Tensors;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using CursoSK.Api.Models;

namespace CursoSK.Api.Services;

#pragma warning disable SKEXP0001
public class VectorStoreService
{
    private readonly ConcurrentDictionary<string, DocumentoVectorial> _store = new();
    private readonly ITextEmbeddingGenerationService? _embeddingService;
    private readonly ILogger<VectorStoreService> _logger;

    public VectorStoreService(Kernel kernel, ILogger<VectorStoreService> logger)
    {
        _embeddingService = kernel.Services.GetService<ITextEmbeddingGenerationService>();
        _logger = logger;
    }

    public async Task<string> IndexarDocumento(string titulo, string contenido, string categoria, string? fuente = null)
    {
        if (_embeddingService == null) throw new InvalidOperationException("Embedding service no configurado.");

        var embedding = await _embeddingService.GenerateEmbeddingAsync(contenido);
        var doc = new DocumentoVectorial
        {
            Titulo = titulo,
            Contenido = contenido,
            Categoria = categoria,
            Fuente = fuente,
            Embedding = embedding
        };
        _store[doc.Id] = doc;
        _logger.LogInformation("Indexado: {Titulo} ({Id})", titulo, doc.Id);
        return doc.Id;
    }

    public Task<List<(DocumentoVectorial Doc, double Score)>> BuscarSimilares(string consulta, int top = 3)
    {
        if (_embeddingService == null) throw new InvalidOperationException("Embedding service no configurado.");
        return BuscarSimilaresInterno(consulta, top);
    }

    private async Task<List<(DocumentoVectorial Doc, double Score)>> BuscarSimilaresInterno(string consulta, int top)
    {
        var queryEmbedding = await _embeddingService!.GenerateEmbeddingAsync(consulta);

        var results = _store.Values
            .Where(d => d.Embedding.HasValue)
            .Select(d => (Doc: d, Score: (double)TensorPrimitives.CosineSimilarity(
                queryEmbedding.Span, d.Embedding!.Value.Span)))
            .OrderByDescending(r => r.Score)
            .Take(top)
            .ToList();

        return results;
    }

    public int Count => _store.Count;
}
#pragma warning restore SKEXP0001
