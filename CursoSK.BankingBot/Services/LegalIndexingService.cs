using Microsoft.SemanticKernel;
using CursoSK.BankingBot.Models;
using CursoSK.BankingBot.Data;
using Microsoft.EntityFrameworkCore;

namespace CursoSK.BankingBot.Services;

/// <summary>Servicio para indexar automáticamente las leyes de la BD en el vector store.</summary>
public class LegalIndexingService
{
    private readonly VectorStoreService _vectorStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LegalIndexingService> _logger;

    public LegalIndexingService(VectorStoreService vectorStore, IServiceProvider serviceProvider, ILogger<LegalIndexingService> logger)
    {
        _vectorStore = vectorStore;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>Indexa todas las leyes de la BD en el vector store para RAG.</summary>
    public async Task IndexarLeyesAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        var leyes = await db.LeyesNormativas.Where(l => l.Vigente).ToListAsync();
        foreach (var ley in leyes)
        {
            await _vectorStore.IndexarDocumento(
                $"{ley.Titulo} ({ley.NumeroLey})",
                ley.Contenido,
                "ley",
                ley.NumeroLey);
            _logger.LogInformation("📚 Ley indexada: {Titulo}", ley.Titulo);
        }

        var articulos = await db.ArticulosLey.Include(a => a.Ley).ToListAsync();
        foreach (var art in articulos)
        {
            await _vectorStore.IndexarDocumento(
                $"{art.NumeroArticulo} - {art.Ley?.Titulo}",
                art.Texto,
                "articulo",
                art.Ley?.NumeroLey);
        }

        _logger.LogInformation("✅ Indexación completada: {Leyes} leyes, {Articulos} artículos", leyes.Count, articulos.Count);
    }
}
