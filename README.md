# Curso: Semantic Kernel + Agentes IA con Azure OpenAI

> **Duración:** 30 horas (10 sesiones × 3 horas)  
> **Plataforma:** .NET 9 · C# · ASP.NET Core Web API · Semantic Kernel 1.48  
> **Nube:** Azure OpenAI · Azure AI Search · Azure App Service · Microsoft Foundry  

---

## Descripción

Curso práctico que cubre desde los fundamentos de **Microsoft Semantic Kernel** hasta la construcción de **agentes de IA completos** desplegados en Azure. Se trabaja **un único proyecto Web API** que crece sesión a sesión, acumulando funcionalidad.

---

## Proyecto

| Proyecto | Descripción | Puerto |
|---|---|---|
| **[CursoSK.Api](CursoSK.Api/)** | API Web de IA — Kernel, Streaming, Blog, Chat, Plugins, Prompting, RAG | `5192` |

---

## Ramas por Sesión

Cada rama es **acumulativa** — contiene todo el código de las sesiones anteriores.

| Rama | Sesión | Tema Principal |
|---|---|---|
| `sesion/01` | Sesión 1 | Fundamentos SK + Setup Azure |
| `sesion/02` | Sesión 2 | Servicios Multimodales (Streaming, Audio, Imágenes) |
| `sesion/03` | Sesión 3 | Workshop Blog + Chat History |
| `sesion/04` | Sesión 4 | Chat Multimodal + Persistencia EF Core |
| `sesion/05` | Sesión 5 | Plugins y Function Calling |
| `sesion/06` | Sesión 6 | Plugins Avanzados + Filtros + OpenAPI |
| `sesion/07` | Sesión 7 | Prompting Avanzado + Templates YAML |
| `sesion/08` | Sesión 8 | Workshop Podcast + Intro Embeddings |
| `sesion/09` | Sesión 9 | RAG Completo + Agent Framework |
| `sesion/10` | Sesión 10 | Deployment Azure + Foundry + Integración Final |
| `main` | — | Estado final (merge de sesion/10) |

```bash
# Cambiar a una sesión específica
git checkout sesion/05
dotnet build CursoSK.Api/CursoSK.Api.csproj
```

---

## Requisitos Previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/) con extensión C#
- Cuenta de [Azure](https://portal.azure.com) con suscripción activa
- [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli) (`az` en PATH)
- Recurso de Azure OpenAI con deployments configurados (ver [Scripts Azure](#scripts-azure))

---

## Inicio Rápido

### 1. Clonar el repositorio

```bash
git clone https://github.com/desarrollossoftware607/CursoSK-BankingBot.git
cd CursoSK-BankingBot
```

### 2. Configurar credenciales

Edita `CursoSK.Api/appsettings.json` con tus credenciales de Azure OpenAI:

```json
{
  "LLMSettings": {
    "Provider": "azure",
    "AzureOpenAI": {
      "DeploymentName": "gpt-35-turbo-16k",
      "Endpoint": "https://TU-RECURSO.openai.azure.com/",
      "ApiKey": "TU-API-KEY"
    }
  }
}
```

### 3. Ejecutar

```bash
cd CursoSK.Api
dotnet run
# Abrir: http://localhost:5192/swagger
```

---

## Scripts Azure

Los scripts PowerShell en `Scripts/Azure/` permiten crear todos los recursos necesarios en Azure.

| Script | Propósito |
|---|---|
| [`00-variables.ps1`](Scripts/Azure/00-variables.ps1) | Variables compartidas y funciones auxiliares |
| [`01-crear-recurso-openai.ps1`](Scripts/Azure/01-crear-recurso-openai.ps1) | Resource Group + Azure OpenAI + deployment chat |
| [`02-crear-deployment-whisper.ps1`](Scripts/Azure/02-crear-deployment-whisper.ps1) | Deployment de audio (Whisper/TTS) |
| [`07-crear-deployment-embedding.ps1`](Scripts/Azure/07-crear-deployment-embedding.ps1) | Deployment de embeddings |
| [`09-crear-ai-search.ps1`](Scripts/Azure/09-crear-ai-search.ps1) | Azure AI Search (opcional, tier free) |
| [`10-deploy-app-service.ps1`](Scripts/Azure/10-deploy-app-service.ps1) | Deploy a Azure App Service |
| [`10-foundry-setup.ps1`](Scripts/Azure/10-foundry-setup.ps1) | Microsoft Foundry — guía de configuración |

```powershell
# Crear todos los recursos desde cero
cd Scripts/Azure
. .\00-variables.ps1
.\01-crear-recurso-openai.ps1
.\02-crear-deployment-whisper.ps1
.\07-crear-deployment-embedding.ps1
```

> **Nota:** Cada script incluye también instrucciones paso a paso para crear los mismos recursos desde el **Portal de Azure**.

---

## Documentación del Curso

| Documento | Contenido |
|---|---|
| [`CURSO_UNIFICADO_JORNALIZACION.md`](CURSO_UNIFICADO_JORNALIZACION.md) | Jornalización completa — 10 sesiones detalladas |
| [`GUION_DIAPOSITIVAS_IA.md`](GUION_DIAPOSITIVAS_IA.md) | Prompts para generar diapositivas con IA |
| [`PASO_A_PASO_CODIGO_FUENTE.md`](PASO_A_PASO_CODIGO_FUENTE.md) | Guía paso a paso del código fuente |

---

## Estructura del Repositorio

```
Curso Agentes/
├── CursoSK.Api/
│   ├── Controllers/
│   │   ├── KernelController.cs        (Sesión 1)
│   │   ├── MultimodalController.cs    (Sesión 2)
│   │   ├── BlogController.cs          (Sesión 3)
│   │   ├── ChatController.cs          (Sesiones 3-4)
│   │   ├── AgentController.cs         (Sesiones 5-6)
│   │   ├── PromptingController.cs     (Sesión 7)
│   │   └── RAGController.cs           (Sesiones 8-9)
│   ├── Services/
│   │   ├── BlogService.cs             (Sesión 3)
│   │   ├── ChatSessionService.cs      (Sesión 3)
│   │   └── VectorStoreService.cs      (Sesión 8)
│   ├── Plugins/
│   │   ├── ClimaPlugin.cs             (Sesión 5)
│   │   └── MathPlugin.cs              (Sesión 5)
│   ├── Filters/
│   │   └── LoggingFilter.cs           (Sesión 6)
│   ├── Models/
│   │   ├── ChatModels.cs              (Sesión 4)
│   │   └── DocumentoVectorial.cs      (Sesión 8)
│   ├── Data/
│   │   └── AppDbContext.cs            (Sesión 4)
│   ├── DTOs/
│   │   └── Requests.cs
│   ├── Prompts/
│   │   └── ClasificarIntencion.yaml   (Sesión 7)
│   ├── Program.cs
│   └── appsettings.json
├── Scripts/
│   └── Azure/
│       ├── 00-variables.ps1
│       ├── 01-crear-recurso-openai.ps1
│       ├── 02-crear-deployment-whisper.ps1
│       ├── 07-crear-deployment-embedding.ps1
│       ├── 09-crear-ai-search.ps1
│       ├── 10-deploy-app-service.ps1
│       └── 10-foundry-setup.ps1
├── CURSO_UNIFICADO_JORNALIZACION.md
├── GUION_DIAPOSITIVAS_IA.md
├── PASO_A_PASO_CODIGO_FUENTE.md
└── README.md
```

---

## Tecnologías

| Tecnología | Versión |
|---|---|
| .NET | 9.0 |
| Semantic Kernel | 1.48.0 |
| EF Core SQLite | 9.0.4 |
| Swashbuckle | 6.9.0 |
| System.Numerics.Tensors | 9.0.1 |
| Azure OpenAI | GPT-3.5/4, DALL-E 3, Whisper, TTS |
| Microsoft Foundry | Portal ai.azure.com |
