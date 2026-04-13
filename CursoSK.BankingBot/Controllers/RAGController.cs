using Microsoft.AspNetCore.Mvc;
using CursoSK.BankingBot.DTOs;
using CursoSK.BankingBot.Services;

namespace CursoSK.BankingBot.Controllers;

/// <summary>RAG — Búsqueda semántica en base vectorial de documentos legales.</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("🔍 RAG / Vector Store")]
public class RAGController : ControllerBase
{
    private readonly VectorStoreService _vectorStore;
    private readonly IWebHostEnvironment _env;

    public RAGController(VectorStoreService vectorStore, IWebHostEnvironment env)
    {
        _vectorStore = vectorStore;
        _env = env;
    }

    /// <summary>Indexa un documento en el vector store para búsqueda semántica.</summary>
    [HttpPost("indexar")]
    public async Task<IActionResult> Indexar([FromBody] IndexarDocumentoRequest request)
    {
        await _vectorStore.IndexarDocumento(request.Titulo, request.Contenido, request.Categoria, request.Fuente);
        return Ok(new { mensaje = $"Documento '{request.Titulo}' indexado exitosamente", categoria = request.Categoria });
    }

    /// <summary>Busca documentos similares a una consulta usando embeddings.</summary>
    [HttpPost("buscar")]
    public async Task<IActionResult> Buscar([FromBody] BusquedaSemanticaRequest request)
    {
        var resultados = await _vectorStore.BuscarSimilares(request.Consulta, request.Top);

        return Ok(new
        {
            consulta = request.Consulta,
            resultados = resultados.Select(r => new
            {
                titulo = r.Doc.Titulo,
                contenido = r.Doc.Contenido,
                categoria = r.Doc.Categoria,
                fuente = r.Doc.Fuente,
                relevancia = r.Score
            })
        });
    }

    /// <summary>Carga automáticamente los archivos .txt de Docs/Leyes, los divide en chunks y los indexa en el vector store.</summary>
    [HttpPost("indexar/leyes")]
    public async Task<IActionResult> IndexarLeyes([FromQuery] int chunkSize = 800, [FromQuery] int overlap = 100)
    {
        var leyesPath = Path.Combine(_env.ContentRootPath, "Docs", "Leyes");
        if (!Directory.Exists(leyesPath))
            return NotFound(new { error = "No se encontró la carpeta Docs/Leyes" });

        var archivos = Directory.GetFiles(leyesPath, "*.txt");
        if (archivos.Length == 0)
            return NotFound(new { error = "No hay archivos .txt en Docs/Leyes" });

        var resumen = new List<object>();

        foreach (var archivo in archivos)
        {
            var contenido = await System.IO.File.ReadAllTextAsync(archivo);
            var nombreArchivo = Path.GetFileNameWithoutExtension(archivo);
            var chunks = ChunkTexto(contenido, chunkSize, overlap);

            for (int i = 0; i < chunks.Count; i++)
            {
                await _vectorStore.IndexarDocumento(
                    titulo: $"{nombreArchivo} — Fragmento {i + 1}/{chunks.Count}",
                    contenido: chunks[i],
                    categoria: "legislacion_bancaria",
                    fuente: Path.GetFileName(archivo));
            }

            resumen.Add(new { archivo = Path.GetFileName(archivo), fragmentos = chunks.Count });
        }

        return Ok(new
        {
            mensaje = $"Se indexaron {archivos.Length} leyes en el vector store",
            chunkSize,
            overlap,
            archivos = resumen
        });
    }

    /// <summary>Sube un archivo de ley (.txt) y lo indexa en el vector store con chunking automático.</summary>
    [HttpPost("indexar/archivo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> IndexarArchivo(
        IFormFile archivo,
        [FromQuery] string categoria = "legislacion_bancaria",
        [FromQuery] int chunkSize = 800,
        [FromQuery] int overlap = 100)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest(new { error = "Debe enviar un archivo" });

        if (!archivo.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { error = "Solo se aceptan archivos .txt" });

        using var reader = new StreamReader(archivo.OpenReadStream());
        var contenido = await reader.ReadToEndAsync();
        var nombreArchivo = Path.GetFileNameWithoutExtension(archivo.FileName);
        var chunks = ChunkTexto(contenido, chunkSize, overlap);

        for (int i = 0; i < chunks.Count; i++)
        {
            await _vectorStore.IndexarDocumento(
                titulo: $"{nombreArchivo} — Fragmento {i + 1}/{chunks.Count}",
                contenido: chunks[i],
                categoria: categoria,
                fuente: archivo.FileName);
        }

        return Ok(new
        {
            mensaje = $"Archivo '{archivo.FileName}' indexado exitosamente",
            fragmentos = chunks.Count,
            categoria,
            chunkSize,
            overlap
        });
    }

    private static List<string> ChunkTexto(string texto, int chunkSize, int overlap)
    {
        var chunks = new List<string>();
        var lineas = texto.Split('\n');
        var buffer = new System.Text.StringBuilder();

        foreach (var linea in lineas)
        {
            if (buffer.Length + linea.Length > chunkSize && buffer.Length > 0)
            {
                chunks.Add(buffer.ToString().Trim());
                // Overlap: conservar las últimas líneas como contexto
                var contenidoActual = buffer.ToString();
                buffer.Clear();
                if (overlap > 0 && contenidoActual.Length > overlap)
                    buffer.Append(contenidoActual[^overlap..]);
            }
            buffer.AppendLine(linea);
        }
        if (buffer.Length > 0)
            chunks.Add(buffer.ToString().Trim());

        return chunks;
    }
}
