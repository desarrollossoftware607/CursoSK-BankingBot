# Guión para Solicitar Diapositivas a una IA

> **Alineado con:** `CURSO_UNIFICADO_JORNALIZACION.md` — 10 sesiones / 30 horas

Copia y pega los siguientes prompts en ChatGPT, Copilot o Claude para generar las diapositivas de cada sesión. Ajusta el tema según la sesión que necesites.

---

## Prompt Base (usar para cada sesión)

```
Actúa como un diseñador instruccional experto en cursos técnicos de programación.

Genera una presentación en formato Markdown (que luego convertiré a PowerPoint con Marp o Slidev) para la siguiente sesión de clase:

## Datos de la sesión:
- **Curso:** Semantic Kernel + Agentes de IA con Azure OpenAI
- **Sesión:** [NÚMERO] de 10
- **Título:** [TÍTULO DE LA SESIÓN]
- **Duración:** 3 horas
- **Audiencia:** Desarrolladores con conocimientos básicos de C#/.NET
- **Enfoque:** "Aprender Haciendo" — Se trabajan DOS proyectos Web API que crecen sesión a sesión: CursoSK.Api (API genérica de IA, puerto 5192) y CursoSK.BankingBot (Bot bancario, puerto 5290)
- **Contexto bancario:** Los participantes trabajan en un banco y quieren aplicar IA a sus procesos
- **Nube:** Azure OpenAI + Microsoft Foundry + Azure App Service
- **Repositorio:** Monorepo con ramas Git por sesión (sesion/01 a sesion/10)

## Requisitos de las diapositivas:
1. Portada con título, número de sesión y fecha
2. Diapositiva de Objetivos de la sesión (3-4 objetivos concretos)
3. Diapositiva de Agenda (bloques de tiempo: Teoría, Azure Setup, Práctica)
4. Contenido teórico: máximo 6 puntos por diapositiva, usar diagramas cuando sea posible
5. Diapositivas de código: mostrar snippets pequeños y legibles (máximo 15 líneas por slide)
6. Diapositiva de "Azure Setup" — pasos para crear/configurar recursos en Azure (portal + CLI)
7. Diapositiva de "Demo en Vivo" con los pasos que el instructor seguirá
8. Diapositiva de "CursoSK.Api" — código y endpoint del proyecto genérico para esta sesión
9. Diapositiva de "CursoSK.BankingBot" — código y endpoint del proyecto bancario para esta sesión
10. Diapositiva de "Así se ve en Producción" — mostrar cómo el concepto se aplica en el proyecto real del laboratorio clínico
11. Diapositiva de "Script Azure" — si la sesión incluye script PowerShell, mostrarlo
12. Diapositiva de Resumen con los 3-5 puntos clave
13. Diapositiva de "Próxima Sesión" — preview de lo que viene
14. Máximo 25 diapositivas por sesión

## Formato de salida:
Usa separadores --- entre cada diapositiva. Usa # para títulos, ## para subtítulos.
Incluye notas del presentador entre <!-- --> cuando sea necesario.

## Contenido específico de la sesión:
[PEGAR AQUÍ EL CONTENIDO DE LA SESIÓN DESDE CURSO_UNIFICADO_JORNALIZACION.md]
```

---

## Prompts Específicos por Sesión

### Sesión 1 — Fundamentos SK + Setup Azure
```
[Usar el prompt base con:]
- Sesión: 1 de 10
- Título: Fundamentos de Semantic Kernel + Setup Azure
- Rama Git: sesion/01
- Contenido: Qué es SK, componentes, arquitectura Kernel→Services→Plugins→Memory, patrón Builder, OpenAI vs Azure OpenAI, Microsoft Foundry overview, creación de ambos proyectos Web API + Swagger, inyección del Kernel en ASP.NET Core, primer endpoint POST /api/kernel/prompt
- Azure Setup: Crear Resource Group + Azure OpenAI + deployment de chat (3 opciones: portal Azure, portal Foundry, Azure CLI)
- Demo CursoSK.Api: Crear proyecto con `dotnet new webapi`, instalar NuGet SK 1.48.0, KernelController con /prompt y /prompt/configurado
- Demo BankingBot: Setup base del proyecto con Kernel apuntando a Azure OpenAI
- Script Azure: 01-crear-recurso-openai.ps1
- Diagrama sugerido: Arquitectura SK (Kernel → Services → Plugins → Memory)
```

### Sesión 2 — Servicios Multimodales
```
[Usar el prompt base con:]
- Sesión: 2 de 10
- Título: Servicios Multimodales — Streaming, Audio e Imágenes
- Rama Git: sesion/02
- Contenido: Chat Completion, Streaming SSE (token por token), DALL-E (generación de imágenes), TTS (texto a audio), Whisper (audio a texto), OpenAIPromptExecutionSettings, #pragma warning disable SKEXP0010
- Azure Setup: Crear deployment de Whisper/gpt-4o-mini-transcribe (puede requerir recurso separado por región)
- Demo CursoSK.Api: MultimodalController con endpoint /stream (SSE), registrar servicios multimodales en Program.cs
- Demo BankingBot: AudioController con /transcribir para transcripción de audio bancario
- Script Azure: 02-crear-deployment-whisper.ps1
- Diagrama sugerido: Flujo request → Kernel → Servicio IA → Response
- Caso bancario: Transcripción de llamadas de servicio al cliente, reportes en audio
```

### Sesión 3 — Workshop Blog + Chat History
```
[Usar el prompt base con:]
- Sesión: 3 de 10
- Título: Workshop — Blog Generator + Chat History
- Rama Git: sesion/03
- Contenido: Service Layer pattern, prompts para HTML Gutenberg, BlogService, BlogController, ChatHistory (3 roles: System/User/Assistant), gestión de sesiones con ConcurrentDictionary, ChatSessionService, ChatController
- Demo CursoSK.Api: POST /api/blog/generar (contenido HTML Gutenberg), POST /api/chat/{id}/mensaje (chat con memoria por sesión), GET /api/chat/{id}/historial
- Demo BankingBot: ConversationService con system prompt bancario, ChatController bancario
- Caso bancario: Generación automática de comunicados y boletines, bot de atención con contexto
```

### Sesión 4 — Chat Multimodal + Persistencia EF Core
```
[Usar el prompt base con:]
- Sesión: 4 de 10
- Título: Chat Multimodal + Persistencia EF Core
- Rama Git: sesion/04
- Contenido: ChatMessageContentItemCollection, ImageContent (URL y upload), GPT-4 Vision, EF Core + SQLite (Code First, Migrations, Seed Data), modelos de datos (ChatSession, ChatMessage, PluginInvocationLog, ContenidoGenerado)
- Demo CursoSK.Api: POST /api/chat/{id}/imagen (URL), POST /api/chat/{id}/imagen/upload (archivo), AppDbContext con seed data
- Demo BankingBot: Modelos bancarios, BankingDbContext
- Diagrama sugerido: Flujo de conversación con ChatHistory + ImageContent
- Caso bancario: Análisis de documentos enviados como imagen (KYC), persistencia de conversaciones
```

### Sesión 5 — Plugins y Function Calling
```
[Usar el prompt base con:]
- Sesión: 5 de 10
- Título: Plugins Nativos y Function Calling Automático
- Rama Git: sesion/05
- Contenido: [KernelFunction], [Description], plugins nativos (ClimaPlugin, MathPlugin), plugins pre-construidos (TimePlugin), FunctionChoiceBehavior.Auto(), registro de plugins (ImportPluginFromObject vs AddFromType), AgentController con /consultar y /plugins
- Demo CursoSK.Api: Crear plugins, registrar en Kernel, POST /api/agent/consultar → ver cómo el LLM decide qué función invocar
- Demo BankingBot: OnboardingPlugin (verificar_identidad, crear_cuenta), CalculadoraFinancieraPlugin, OnboardingController
- Diagrama sugerido: LLM → analiza prompt → decide Tool Call → ejecuta función → retorna resultado
- Caso bancario: Plugin para verificar identidad, apertura de cuentas, cálculos financieros
```

### Sesión 6 — Plugins Avanzados + Filtros + OpenAPI
```
[Usar el prompt base con:]
- Sesión: 6 de 10
- Título: Plugins Avanzados + Filtros + OpenAPI
- Rama Git: sesion/06
- Contenido: AddFromObject vs AddFromType (DI en plugins), HttpClient inyectado en plugins, IFunctionInvocationFilter (LoggingFilter), interceptores de auditoría, plugins OpenAPI (ImportPluginFromOpenApiAsync)
- Demo CursoSK.Api: LoggingFilter que registra cada invocación de función con tiempo de ejecución
- Demo BankingBot: LegalPlugin con HttpClient inyectado, AuditFilter para auditoría, PrestamosController, LegalController
- Caso bancario: Plugin que consulta API del core bancario, filtros de auditoría para compliance
```

### Sesión 7 — Prompting Avanzado + Templates YAML
```
[Usar el prompt base con:]
- Sesión: 7 de 10
- Título: Prompting Avanzado + Templates YAML
- Rama Git: sesion/07
- Azure Setup: Crear deployment de embeddings (text-embedding-3-small) como preparación para sesión 8
- Contenido: Técnicas Zero-Shot, Few-Shot, Chain of Thought, Generated Knowledge; templates SK nativo/Handlebars/Liquid/YAML; archivos YAML con execution_settings, input_variables; KernelFunctionYaml.ToPromptTemplateConfig(); KernelFunctionFactory.CreateFromPrompt()
- Demo CursoSK.Api: PromptingController con 4 endpoints (zero-shot, few-shot, chain-of-thought, yaml), archivo Prompts/ClasificarIntencion.yaml
- Demo BankingBot: Templates YAML para ClasificarIntencion bancaria y AnalisisDocumento
- Script Azure: 07-crear-deployment-embedding.ps1
- Diagrama sugerido: Comparativa visual de Zero-Shot vs Few-Shot vs CoT
- Caso bancario: Clasificación automática de solicitudes, análisis de sentimiento de quejas
```

### Sesión 8 — Workshop Podcast + Intro Embeddings
```
[Usar el prompt base con:]
- Sesión: 8 de 10
- Título: Workshop Podcast + Introducción a Embeddings
- Rama Git: sesion/08
- Contenido: Pipeline de generación datos→markdown→brainstorm→script→audio; ¿Qué es un embedding? (vectores de 1536 dimensiones); similaridad coseno; VectorStoreService con ConcurrentDictionary + TensorPrimitives.CosineSimilarity; DocumentoVectorial; ITextEmbeddingGenerationService
- Demo CursoSK.Api: VectorStoreService + DocumentoVectorial, registrar servicio de embeddings en Program.cs, indexar textos de prueba, buscar por similaridad
- Demo BankingBot: VectorStoreService para indexar normativas bancarias
- Diagrama sugerido: Texto → Embedding Model → Vector [1536 dims] → Cosine Similarity → Score
- Caso bancario: Vectorizar regulaciones CNBS para búsqueda semántica
```

### Sesión 9 — RAG Completo + Agent Framework
```
[Usar el prompt base con:]
- Sesión: 9 de 10
- Título: RAG Completo + Agent Framework
- Rama Git: sesion/09
- Azure Setup (opcional): Crear Azure AI Search con tier free
- Contenido: Patrón RAG completo (Retrieve→Augment→Generate), indexación de documentos, chunking, búsqueda vectorial, inyección de contexto, reducción de alucinaciones, RAG en Foundry vs RAG en código (comparativa), ChatCompletionAgent del Agent Framework, Foundry guardrails (Content Safety)
- Demo CursoSK.Api: RAGController con /indexar, /buscar, /consultar, /seed (FAQs de ejemplo)
- Demo BankingBot: RAGController con /indexar/leyes (leyes hondureñas), LegalController con /consultar/rag, LegalIndexingService
- Demo Foundry: Subir documentos al Playground → ver índice vectorial automático → comparar con RAG en código
- Script Azure: 09-crear-ai-search.ps1
- Diagrama sugerido: Flujo RAG (Query → Embed → Vector Search → Context + LLM → Response)
- Caso bancario: Base de conocimiento legal bancario, consulta de normativas CNBS con citaciones
```

### Sesión 10 — Deployment Azure + Foundry + Integración Final
```
[Usar el prompt base con:]
- Sesión: 10 de 10
- Título: Deployment Azure + Foundry + Integración Final
- Rama Git: sesion/10 (merge a main)
- Contenido: Azure App Service (PaaS .NET), dotnet publish + zip deploy, variables de entorno para producción, Microsoft Foundry completo (Playground, File Upload, Guardrails, Agent Service), 3 tipos de agentes Foundry (Solicitud, Flujo de Trabajo, Hospedados), demo end-to-end de BankingBot
- Azure Setup: Crear App Service Plan + Web App, configurar variables de entorno, zip deploy
- Demo: Deploy CursoSK.Api a Azure App Service, probar Swagger en URL pública, demo completa BankingBot (todos los endpoints), Foundry: subir documentos y crear índice vectorial, merge a main
- Scripts Azure: 10-deploy-app-service.ps1, 10-foundry-setup.ps1
- Caso bancario: Deployment de producción del bot bancario completo con todos los módulos
```

---

## Diapositivas Adicionales — Microsoft Foundry y Carga de Archivos Web

### Diapositiva Extra: Carga de Archivos en Foundry (Índices Vectoriales desde el Portal)
```
[Usar el prompt base con:]
- Título: Carga de Archivos y RAG desde el Portal de Microsoft Foundry
- Contexto: Este bloque complementa las sesiones 8-9 del curso (Embeddings y RAG)
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
Necesito que generes el contenido completo para una presentación PowerPoint de la Sesión [N] del curso "Semantic Kernel + Agentes de IA con Azure OpenAI".

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
