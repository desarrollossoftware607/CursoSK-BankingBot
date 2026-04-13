using Microsoft.AspNetCore.Mvc;
using CursoSK.Api.DTOs;
using CursoSK.Api.Services;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("3️⃣ Blog — Sesión 3")]
public class BlogController : ControllerBase
{
    private readonly BlogService _blogService;
    public BlogController(BlogService blogService) => _blogService = blogService;

    /// <summary>Genera contenido HTML con bloques Gutenberg para un blog post.</summary>
    [HttpPost("generar")]
    public async Task<IActionResult> GenerarBlogPost([FromBody] BlogRequest request)
    {
        var contenido = await _blogService.GenerarContenidoBlog(request.Tema);
        return Ok(new { tema = request.Tema, contenidoHtml = contenido });
    }
}
