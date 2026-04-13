namespace CursoSK.BankingBot.Models;

/// <summary>Documento vectorizado para búsqueda semántica (RAG).</summary>
public class DocumentoVectorial
{
    public string Id { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string? Fuente { get; set; }
    public ReadOnlyMemory<float>? Embedding { get; set; }
}
