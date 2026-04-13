# Banking Bot - Semantic Kernel API

Proyecto de demostración para el curso de **Semantic Kernel** aplicado al sector bancario.

## 🏦 Caso de Uso

Asistente inteligente de onboarding bancario que:
- Guía al cliente en solicitudes de préstamo
- Verifica requisitos y documentos
- Consulta normativas legales via RAG
- Calcula cuotas y evalúa elegibilidad
- Persiste conversaciones en SQLite

## 🛠️ Stack Tecnológico

| Componente | Tecnología |
|-----------|------------|
| Framework | ASP.NET Core 9.0 |
| IA / LLM | Microsoft Semantic Kernel + Azure OpenAI |
| Base de datos | SQLite (EF Core) |
| Vector Store | InMemory (upgradeable a Qdrant, Azure AI Search) |
| Documentación | Swagger / OpenAPI |

## 📁 Estructura del Proyecto

```
CursoSK.BankingBot/
├── Controllers/
│   ├── OnboardingController.cs   → Asistente de préstamos con Function Calling
│   ├── LegalController.cs        → Asistente legal con RAG
│   ├── ChatController.cs         → Chat con historial persistente
│   ├── RAGController.cs          → Indexación y búsqueda vectorial
│   └── PrestamosController.cs    → CRUD de solicitudes y clientes
├── Plugins/
│   ├── OnboardingPlugin.cs       → Requisitos, simulación, elegibilidad
│   ├── LegalPlugin.cs            → Normativas, KYC, riesgo legal
│   └── CalculadoraFinancieraPlugin.cs → Cuotas, amortización, conversión
├── Services/
│   ├── ConversationService.cs    → Chat con persistencia SQLite
│   ├── VectorStoreService.cs     → Embeddings + búsqueda semántica
│   └── LegalIndexingService.cs   → Indexación automática de leyes
├── Models/
│   ├── ClienteModels.cs          → Cliente, DocumentoCliente
│   ├── PrestamoModels.cs         → SolicitudPrestamo, Requisitos, Historial
│   ├── ChatModels.cs             → ChatSession, ChatMessage, PluginLog
│   ├── LegalModels.cs            → LeyNormativa, ArticuloLey
│   └── DocumentoVectorial.cs     → Modelo para vector store
├── Data/
│   └── BankingDbContext.cs       → Context con seed data completo
├── DTOs/
│   └── Requests.cs               → Request models
├── Filters/
│   └── AuditFilter.cs            → Auditoría de plugins
├── Prompts/
│   ├── ClasificarIntencion.yaml  → Clasificación de intenciones
│   └── AnalisisDocumento.yaml    → Análisis de documentos OCR
└── Program.cs                    → Configuración DI + Kernel + Swagger
```

## 🚀 Inicio Rápido

```bash
# 1. Configurar API Key en appsettings.json
# 2. Restaurar y ejecutar
dotnet restore
dotnet run

# 3. Abrir Swagger
# http://localhost:5000/swagger
```

## 🔀 Ramas del Repositorio

| Rama | Contenido |
|------|-----------|
| `main` | Proyecto completo |
| `01-setup-kernel` | Sesión 1: Proyecto base + Kernel |
| `02-modelos-db` | Sesión 2: Modelos + EF Core + Seed Data |
| `03-plugins-onboarding` | Sesión 3: Plugins de onboarding |
| `04-plugins-legal` | Sesión 4: Plugin legal + calculadora |
| `05-chat-persistente` | Sesión 5: Chat con historial SQLite |
| `06-function-calling` | Sesión 6: Function Calling automático |
| `07-prompts-templates` | Sesión 7: Prompt templates YAML |
| `08-rag-vectorstore` | Sesión 8: RAG + Vector Store |
| `09-filtros-auditoria` | Sesión 9: Filtros + Auditoría |
| `10-integracion-completa` | Sesión 10: Integración final |

## 📜 Normativas Incluidas (Seed Data)

- Ley del Sistema Financiero (Decreto 129-2004)
- Ley Contra el Lavado de Activos (Decreto 144-2014)
- Ley de Protección al Consumidor Financiero (Decreto 170-2016)
- Normas de Gestión de Riesgo Crediticio (Resolución CNBS-GES-041-2019)

## 📝 Licencia

Proyecto educativo para curso de Semantic Kernel.
