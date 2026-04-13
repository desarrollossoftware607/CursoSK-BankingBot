namespace CursoSK.Api.Models;

public class DocumentoVectorial
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string? Fuente { get; set; }
    public ReadOnlyMemory<float>? Embedding { get; set; }
}
