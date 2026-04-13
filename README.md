# Curso: Semantic Kernel + Agentes IA con Azure OpenAI

> **Duración:** 30 horas (10 sesiones × 3 horas)  
> **Plataforma:** .NET 9 · C# · ASP.NET Core Web API · Semantic Kernel 1.48  
> **Nube:** Azure OpenAI · Azure AI Search · Azure App Service · Microsoft Foundry  

---

## Descripción

Curso práctico que cubre desde los fundamentos de **Microsoft Semantic Kernel** hasta la construcción de **agentes de IA completos** desplegados en Azure. Se trabajan **dos proyectos reales** a lo largo de las 10 sesiones, cada uno con un enfoque distinto.

---

## Proyectos

| Proyecto | Descripción | Puerto |
|---|---|---|
| **[CursoSK.Api](CursoSK.Api/)** | API genérica de IA — Blog, Chat, Streaming, Plugins, Prompting, RAG | `5192` |
| **[CursoSK.BankingBot](CursoSK.BankingBot/)** | Bot bancario especializado — Onboarding, Préstamos, Legal, Audio, RAG leyes | `5290` |

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

Cada proyecto usa `appsettings.json` con la sección `LLMSettings`. Configura tus credenciales de Azure OpenAI:

**CursoSK.Api/appsettings.json:**
```json
{
  "LLMSettings": {
    "Provider": "AzureOpenAI",
    "AzureOpenAI": {
      "Endpoint": "https://TU-RECURSO.openai.azure.com/",
      "ApiKey": "TU-API-KEY",
      "ChatDeployment": "gpt-35-turbo-16k",
      "EmbeddingDeployment": "text-embedding-3-small"
    }
  }
}
```

**CursoSK.BankingBot/appsettings.json:** Configuración similar en la sección `AzureOpenAI`.

### 3. Ejecutar un proyecto

```bash
# Proyecto genérico (API)
cd CursoSK.Api
dotnet run
# Abrir: http://localhost:5192/swagger

# Bot bancario
cd CursoSK.BankingBot
dotnet run
# Abrir: http://localhost:5290/swagger
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
# Ejemplo: crear todos los recursos desde cero
cd Scripts/Azure
. .\00-variables.ps1       # Cargar variables
.\01-crear-recurso-openai.ps1
.\02-crear-deployment-whisper.ps1
.\07-crear-deployment-embedding.ps1
```

> **Nota:** Cada script incluye también instrucciones paso a paso para crear los mismos recursos desde el **Portal de Azure** (como comentarios en el código).

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
├── CursoSK.Api/                    # Proyecto 1: API genérica de IA
│   ├── Controllers/                # 7 controllers (Kernel, Multimodal, Blog, Chat, Agent, Prompting, RAG)
│   ├── Services/                   # BlogService, ChatSessionService, VectorStoreService
│   ├── Plugins/                    # ClimaPlugin, MathPlugin
│   ├── Filters/                    # LoggingFilter (FunctionInvocationFilter)
│   ├── Models/                     # Entidades EF Core
│   ├── DTOs/                       # Request records
│   ├── Data/                       # AppDbContext (SQLite)
│   ├── Prompts/                    # Templates YAML (Handlebars)
│   └── Program.cs                  # Startup: SK + EF Core + Swagger
│
├── CursoSK.BankingBot/             # Proyecto 2: Bot bancario
│   ├── Controllers/                # Onboarding, Chat, Legal, Audio, RAG, Prestamos
│   ├── Services/                   # ConversationService, LegalIndexingService, VectorStoreService
│   ├── Plugins/                    # OnboardingPlugin, CalculadoraFinancieraPlugin, LegalPlugin
│   ├── Docs/Leyes/                 # Leyes hondureñas para RAG
│   └── Program.cs
│
├── Scripts/Azure/                  # Scripts PowerShell para crear recursos Azure
│
├── CURSO_UNIFICADO_JORNALIZACION.md
├── GUION_DIAPOSITIVAS_IA.md
├── PASO_A_PASO_CODIGO_FUENTE.md
└── README.md                       # ← Estás aquí
```

---

## Tecnologías

- **Microsoft Semantic Kernel** 1.48.0 — Orquestación de IA
- **ASP.NET Core 9.0** — Web API REST
- **Entity Framework Core** 9.0 + SQLite — Persistencia
- **Swashbuckle** 6.9.0 — Swagger UI
- **Azure OpenAI** — GPT-3.5/4, DALL-E, Whisper, Embeddings
- **Azure AI Search** — Búsqueda vectorial (opcional)
- **Azure App Service** — Hosting en producción
- **Microsoft Foundry** — Playground, guardrails, Agent Service

---

## Licencia

Material educativo. Uso autorizado exclusivamente para los participantes del curso.
