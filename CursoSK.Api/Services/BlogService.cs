using Microsoft.SemanticKernel;

namespace CursoSK.Api.Services;

public class BlogService
{
    private readonly Kernel _kernel;
    public BlogService(Kernel kernel) => _kernel = kernel;

    public async Task<string> GenerarContenidoBlog(string tema)
    {
        var blogPrompt = $"""
            Genera una publicación de blog detallada acerca de {tema}.
            Incluye introducción, varios párrafos, code snippets si aplica, y conclusión.
            Separa cada sección con un encabezado.
            Usa los siguientes bloques Gutenberg de WordPress:

            Para encabezado: <!-- wp:heading --><h2 class="wp-block-heading">TEXTO</h2><!-- /wp:heading -->
            Para párrafo: <!-- wp:paragraph --><p>TEXTO</p><!-- /wp:paragraph -->
            Para lista: <!-- wp:list --><ul class="wp-block-list"><li>ITEM</li></ul><!-- /wp:list -->
            Para código: <!-- wp:code --><pre class="wp-block-code"><code>CÓDIGO</code></pre><!-- /wp:code -->
            """;
        var result = await _kernel.InvokePromptAsync(blogPrompt);
        return result.ToString();
    }
}
