# Guión para Solicitar Diapositivas a una IA

Copia y pega los siguientes prompts en ChatGPT, Copilot o Claude para generar las diapositivas de cada sesión. Ajusta el tema según la sesión que necesites.

---

## Prompt Base (usar para cada sesión)

```
Actúa como un diseñador instruccional experto en cursos técnicos de programación.

Genera una presentación en formato Markdown (que luego convertiré a PowerPoint con Marp o Slidev) para la siguiente sesión de clase:

## Datos de la sesión:
- **Curso:** Máster en Semantic Kernel — Creando Proyectos con IA
- **Sesión:** [NÚMERO] de 10
- **Título:** [TÍTULO DE LA SESIÓN]
- **Duración:** 3 horas
- **Audiencia:** Desarrolladores con conocimientos básicos de C#/.NET (algunos pueden usar Python o Java)
- **Enfoque:** "Aprender Haciendo" — El proyecto es una ASP.NET Core Web API con Swagger que crece sesión a sesión
- **Contexto bancario:** Los participantes trabajan en un banco y quieren aplicar IA a sus procesos

## Requisitos de las diapositivas:
1. Portada con título, número de sesión y fecha
2. Diapositiva de Objetivos de la sesión (3-4 objetivos concretos)
3. Diapositiva de Agenda (bloques de tiempo)
4. Contenido teórico: máximo 6 puntos por diapositiva, usar diagramas cuando sea posible
5. Diapositivas de código: mostrar snippets pequeños y legibles (máximo 15 líneas por slide)
6. Diapositiva de "Demo en Vivo" con los pasos que el instructor seguirá
7. Diapositiva de Ejercicio Práctico con instrucciones claras
8. Diapositiva de "Así se ve en Producción" — mostrar cómo el concepto se aplica en el proyecto real del laboratorio clínico
9. Diapositiva de "Conexión Bancaria" — cómo el concepto se aplica al caso de uso del banco
10. Diapositiva de Resumen con los 3-5 puntos clave
11. Diapositiva de "Próxima Sesión" — preview de lo que viene
12. Máximo 25 diapositivas por sesión

## Formato de salida:
Usa separadores --- entre cada diapositiva. Usa # para títulos, ## para subtítulos.
Incluye notas del presentador entre <!-- --> cuando sea necesario.

## Contenido específico de la sesión:
[PEGAR AQUÍ EL CONTENIDO DE LA SESIÓN DESDE EL DOCUMENTO DE JORNALIZACIÓN]
```

---

## Prompts Específicos por Sesión

### Sesión 1 — Fundamentos y El Kernel Core
```
[Usar el prompt base con:]
- Sesión: 1 de 10
- Título: Fundamentos de Semantic Kernel y Configuración del Kernel
- Contenido: Qué es SK, componentes, patrón Builder, API Keys OpenAI/Azure, creación del proyecto Web API con Swagger, inyección del Kernel en ASP.NET Core, primeros endpoints POST /api/kernel/prompt
- Demo: Crear proyecto desde cero con `dotnet new webapi`, instalar NuGet, configurar Program.cs, probar primer endpoint en Swagger
- Diagrama sugerido: Arquitectura SK (Kernel → Services → Plugins → Memory)
```

### Sesión 2 — Servicios Multimodales
```
[Usar el prompt base con:]
- Sesión: 2 de 10
- Título: Servicios Multimodales — Chat, Imágenes, Audio
- Contenido: Chat Completion, Streaming SSE, DALL-E (generación de imágenes), TTS (texto a audio), Whisper (audio a texto), #pragma warning disable SKEXP0010
- Demo: Agregar MultimodalController con endpoints /stream, /imagen, /tts, /stt y probar desde Swagger
- Diagrama sugerido: Flujo request → Kernel → Servicio IA → Response
- Caso bancario: Transcripción de llamadas de atención al cliente, generación de reportes en audio
```

### Sesión 3 — Workshop Blog Post Generator
```
[Usar el prompt base con:]
- Sesión: 3 de 10
- Título: Workshop — Generador de Blog Posts con WordPress
- Contenido: Service Layer pattern, prompts para HTML Gutenberg, BlogService, BlogController, generación de imagen destacada, publicación en WordPress REST API
- Demo: Endpoint POST /api/blog/generar que genera contenido HTML + imagen con DALL-E
- Caso bancario: Generación automática de comunicados internos, boletines informativos con IA
```

### Sesión 4 — Chat con Historia y Multimodal
```
[Usar el prompt base con:]
- Sesión: 4 de 10
- Título: Chat con Historia + Chat Multimodal
- Contenido: IChatCompletionService, ChatHistory (System/User/Assistant), gestión de sesiones con ConcurrentDictionary, ChatMessageContentItemCollection, ImageContent, upload de imágenes vía API
- Demo: ChatController con sesiones por ID, enviar mensajes, enviar imágenes, ver historial
- Diagrama sugerido: Flujo de conversación con ChatHistory
- Caso bancario: Bot de atención al cliente que recuerda contexto, análisis de documentos enviados como imagen
```

### Sesión 5 — Plugins y Function Calling
```
[Usar el prompt base con:]
- Sesión: 5 de 10
- Título: Plugins Nativos y Function Calling Automático
- Contenido: [KernelFunction], [Description], plugins nativos (ClimaPlugin, MathPlugin), FunctionChoiceBehavior.Auto(), AgentController, endpoint GET /plugins para listar funciones
- Demo: Crear plugins, registrar en Kernel, consultar desde Swagger y ver cómo el LLM decide qué función invocar
- Diagrama sugerido: LLM → analiza prompt → decide Tool Call → ejecuta función → retorna resultado
- Caso bancario: Plugin para consultar saldos, otro para verificar estado de trámites
```

### Sesión 6 — Plugins Avanzados
```
[Usar el prompt base con:]
- Sesión: 6 de 10
- Título: Plugins Avanzados — DI, Filters y OpenAPI
- Contenido: AddFromObject vs AddFromType, WikipediaPlugin con HttpClient, IFunctionInvocationFilter (LoggingFilter), plugins OpenAPI
- Demo: Agregar filtro de logging, importar plugin OpenAPI desde Swagger de API externa
- Caso bancario: Plugin que consulta API del core bancario, filtros de auditoría para todas las invocaciones de IA
```

### Sesión 7 — Workshop Video Extractor
```
[Usar el prompt base con:]
- Sesión: 7 de 10
- Título: Workshop — Extracción de Datos de Video
- Contenido: FFmpeg, CliWrap, VideoPlugin, extracción de audio, compresión, transcripción con Whisper, corte de clips, subtítulos
- Demo: VideoController con endpoints /transcribir y /cortar, upload de video desde Swagger
- Caso bancario: Transcripción de reuniones de comité de crédito, análisis de videollamadas con clientes
```

### Sesión 8 — Prompting Avanzado
```
[Usar el prompt base con:]
- Sesión: 8 de 10
- Título: Técnicas de Prompting y Plantillas
- Contenido: Zero-Shot, Few-Shot, Chain of Thought, templates SK/Handlebars/Liquid, archivos YAML, PromptingController con endpoints por técnica
- Demo: Probar cada técnica desde Swagger, cargar prompt YAML de clasificación de intenciones
- Diagrama sugerido: Comparativa visual de Zero-Shot vs Few-Shot vs CoT
- Caso bancario: Clasificación automática de solicitudes (crédito/reclamo/consulta), análisis de sentimiento de quejas
```

### Sesión 9 — Workshop Podcast Generator
```
[Usar el prompt base con:]
- Sesión: 9 de 10
- Título: Workshop — Generador de Podcasts
- Contenido: PodcastService, ReverseMarkdown, pipeline de prompts (brainstorm → script → audio), plantillas YAML, PodcastController
- Demo: POST /api/podcast/generar con URLs de referencia, descargar MP3 generado
- Caso bancario: Generación de resúmenes ejecutivos en audio, briefings matutinos automatizados
```

### Sesión 10 — Vector Stores y RAG
```
[Usar el prompt base con:]
- Sesión: 10 de 10
- Título: Vector Stores, Embeddings y RAG
- Contenido: Embeddings, InMemoryVectorStore, VectorStoreTextSearch, VectorStoreService, RAGController, seed de FAQs, búsqueda vectorial vs RAG con Function Calling
- Demo: Cargar FAQs con /faq/seed, buscar con /buscar, consultar con RAG en /consultar
- Diagrama sugerido: Flujo RAG (Query → Embedding → Vector Search → Context + LLM → Response)
- Caso bancario: Base de conocimiento de regulaciones bancarias, FAQ de productos financieros, búsqueda semántica en políticas internas
```

---

## Diapositivas Adicionales — Microsoft Foundry y Carga de Archivos Web

### Diapositiva Extra: Carga de Archivos en Foundry (Índices Vectoriales desde el Portal)
```
[Usar el prompt base con:]
- Título: Carga de Archivos y RAG desde el Portal de Microsoft Foundry
- Contexto: Este bloque complementa las sesiones de Embeddings y RAG (Días 6-8 del curso de Agentes)
- Contenido:
  1. **Qué pasa cuando subes un archivo en Foundry:** El archivo NO se usa como contexto directo del modelo. Se convierte en embeddings y se almacena en un índice vectorial (Azure AI Search). El modelo luego consulta el índice para recuperar solo los fragmentos relevantes (RAG).
  2. **Paso a paso del flujo:**
     - Ir al Área de Juegos (Playground) en ai.azure.com
     - Seleccionar deployment del modelo → "Adjuntar archivos"
     - Opción de índice: "Crear un nuevo índice" o usar existente
     - Arrastrar archivos (.pdf, .txt, .docx, .md)
     - Clic en "Adjuntar" → Foundry procesa: extracción → chunking → embedding → indexación
  3. **Comparativa visual:**
     - SIN índice vectorial: 50 páginas → 50,000 tokens consumidos del modelo ❌
     - CON índice vectorial: 50 páginas → 200 fragmentos indexados → solo 3-5 relevantes inyectados (~1,000 tokens) ✅
  4. **Límites de protección (Guardrails):**
     - Microsoft.Default vs Microsoft.DefaultV2
     - Filtros: Odio, Autolesiones, Sexual, Violencia (Bloqueo medio)
     - Filtro de liberación de prompt (Prompt Injection)
     - Se asignan al deployment del modelo
  5. **Servicio de Agentes de Foundry — 3 tipos:**
     - Agentes de Solicitud (sin código, portal)
     - Agentes de Flujo de Trabajo (YAML, multi-agente)  
     - Agentes Hospedados (contenedor, SK/LangGraph)
  6. **Demo en vivo:**
     - Subir leyes bancarias hondureñas al Playground
     - Hacer preguntas legales → observar citaciones del índice
     - Comparar con RAG desde código (POST /api/rag/buscar)
- Diagrama sugerido: Flujo de archivo → chunks → embeddings → vector index → query → respuesta
- Caso bancario: Subir normativas CNBS como archivos → consultar regulaciones desde el Playground → comparar con el enfoque de código del proyecto CursoSK.BankingBot
- Referencias:
  - https://learn.microsoft.com/en-us/azure/foundry/openai/how-to/embeddings?tabs=csharp
  - https://learn.microsoft.com/es-mx/azure/foundry/agents/overview
  - https://learn.microsoft.com/es-mx/azure/foundry/
```

---

## Prompt para Generar PowerPoint Directamente (alternativo)

```
Necesito que generes el contenido completo para una presentación PowerPoint de la Sesión [N] del curso "Máster en Semantic Kernel".

Formato de salida: JSON con la siguiente estructura para que yo lo importe con python-pptx:

{
  "titulo": "Sesión N — Título",
  "slides": [
    {
      "layout": "titulo|contenido|codigo|dos_columnas|imagen",
      "titulo": "Título del slide",
      "contenido": ["Bullet 1", "Bullet 2"],
      "codigo": "// código si aplica",
      "notas": "Notas del presentador"
    }
  ]
}

[PEGAR CONTENIDO DE LA SESIÓN]
```

---

## Prompt para Generar Diagramas (Mermaid)

```
Genera diagramas Mermaid para la Sesión [N] del curso de Semantic Kernel. Necesito:

1. Diagrama de arquitectura del componente principal de la sesión
2. Diagrama de secuencia del flujo request → API → Kernel → LLM → Response
3. Diagrama de flujo del ejercicio práctico

Formato: bloques de código ```mermaid``` que pueda pegar directamente en las diapositivas.

Contexto de la sesión:
[PEGAR CONTENIDO DE LA SESIÓN]
```
