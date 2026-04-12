# Curso: Desarrollo de Agentes de IA Multimodales con Microsoft Azure

## Información General

| Campo | Detalle |
|-------|---------|
| **Duración** | 30 horas cronológicas (4 semanas, Lunes a Viernes) |
| **Inversión** | $1,100 USD por participante (mínimo 5) |
| **Modalidad** | Formación práctica de alto nivel — "Aprender Haciendo" |
| **Requisitos** | Experiencia en lenguajes OOP fuertemente tipados, VS Code instalado, suscripción Azure (o crédito $200 nuevos usuarios) |

## Objetivo General

Al finalizar el curso, los participantes serán capaces de **crear agentes de IA multimodales** (documentos, imágenes, audios) que resuelvan problemas de negocio del mundo real. Sabrán crear servicios, tomar decisiones de arquitectura, programarlos y desplegarlos en producción — 100% sobre el ecosistema Microsoft Azure.

## Proyecto de Referencia en Producción

El curso se apoya en un **sistema real en producción**: un agente conversacional multicanal (WhatsApp, Messenger, Chat Web) para un laboratorio clínico, construido con:

| Componente Implementado | Tecnología | Archivos de Referencia |
|--------------------------|------------|------------------------|
| Kernel de Semantic Kernel con Azure OpenAI GPT-4 | `Microsoft.SemanticKernel 1.47.0` | `Program.cs` (líneas 230-340) |
| 6 Plugins nativos en C# (Cotización, Citas, Domicilio, Análisis, Laboratorio, FechaHora) | Native Functions + Auto Function Calling | `API/Plugins/*.cs` |
| Prompt Templates con variables | `skprompt.txt` + `config.json` | `API/Prompts/LabAnalisis/` |
| Transcripción de audio con Whisper | Azure OpenAI (gpt-4o-mini-transcribe) | `CotizacionIntegration.cs` |
| Gestión de estado conversacional por sesión | ConversationSession per phone number | `CotizacionIntegration.cs` |
| Almacenamiento multimedia en Azure Blob Storage | BlobService con SAS tokens | `API/class/BlobService.cs` |
| Integración WhatsApp Cloud API bidireccional | Webhook + Send Messages + Templates | `WhatsappController.cs`, `WhatsAppChatController.cs` |
| Generación de PDF y envío de documentos | docx → PDF → Blob → WhatsApp | `WhatsappController.cs` |
| Multi-LLM (Azure OpenAI / Google Gemini) | Selección dinámica por configuración | `Program.cs`, `CotizacionIntegration.cs` |
| Clasificación de intenciones del usuario | Prompt DeterminarIntencion | `Prompts/LabAnalisis/DeterminarIntencion/` |

---

## Jornalización Detallada

---

### SEMANA 1: Fundamentos — Del Kernel al Plugin Inteligente

---

#### 📅 Día 1 (Lunes) — Fundamentos y Arquitectura de Agentes

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Fundamentos y Arquitectura de Agentes |
| **Subtemas** | Conceptos de IA Generativa, arquitectura de agentes modernos, configuración de entornos .NET/C# y conectores de Azure OpenAI |
| **Herramientas Microsoft IA** | Azure OpenAI Service, Semantic Kernel SDK, Visual Studio / VS Code |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Qué es un agente de IA vs. un chatbot simple
- Arquitectura moderna: Kernel → Plugins → Planners → Memory
- Azure OpenAI Service: modelos disponibles (GPT-4, GPT-4o, GPT-4.1-mini)
- Semantic Kernel como orquestador: por qué Microsoft lo creó

**Actividad Práctica — Aprender Haciendo:**
> Configuración del Kernel básico y primera conexión con modelos GPT-4 en Azure.

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo está configurado el Kernel en `Program.cs` (líneas 230-340):
```csharp
var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt-35-turbo-16k",
    endpoint: "https://agentelabmendez.cognitiveservices.azure.com/",
    apiKey: configuration["IAkey:keygpt"]
);
```
Los participantes replican esta configuración para el dominio bancario, conectándose a su propio recurso Azure OpenAI.

---

#### 📅 Día 2 (Martes) — Prompt Engineering y Plantillas

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Prompt Engineering y Plantillas |
| **Subtemas** | Técnicas de prompting, manejo de plantillas (YAML, Handlebars, Liquid), variables y configuración de parámetros |
| **Herramientas Microsoft IA** | Azure OpenAI (GPT-4), Semantic Kernel (Prompt Templates) |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Zero-shot, few-shot, chain-of-thought prompting
- System prompts efectivos: instrucciones, prohibiciones, flujo obligatorio
- Plantillas con variables `{{$variable}}` en Semantic Kernel
- Archivos `skprompt.txt` + `config.json` como unidad de prompt

**Actividad Práctica — Aprender Haciendo:**
> Diseño de prompts estructurados para el razonamiento legal y cumplimiento bancario.

**🔗 Conexión con el Proyecto Real:**
Se analiza el system prompt del agente de cotizaciones en `CotizacionIntegration.cs`:
```
"Eres un asistente de laboratorio clínico del Laboratorio Méndez.
FLUJO OBLIGATORIO:
1) Identifica al cliente por número de identidad
2) Busca precios
3) Guarda cotización
PROHIBICIONES:
- NUNCA interpretes resultados de análisis
- NUNCA des diagnósticos médicos"
```
Se estudian las plantillas en `Prompts/LabAnalisis/` (BuscarAnalisis, DeterminarIntencion, ExtraerDatosCliente, InfoLaboratorio) y los participantes crean prompts equivalentes para análisis de contratos bancarios.

---

#### 📅 Día 3 (Miércoles) — Plugins Nativos y Funciones en C#

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Plugins Nativos y Funciones en C# |
| **Subtemas** | Creación de Native Functions, lógica de negocio en C#, automatización de procesos e invocación de funciones (Function Calling) |
| **Herramientas Microsoft IA** | Semantic Kernel (Native Plugins), .NET / C# |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- `[KernelFunction]` y `[Description]` — cómo el LLM "ve" las funciones
- Auto Function Calling: `ToolCallBehavior.AutoInvokeKernelFunctions`
- Diseño de plugins: granularidad, naming, parámetros descriptivos
- Registro de plugins: `ImportPluginFromObject()` vs `CreatePluginFromPromptDirectory()`

**Actividad Práctica — Aprender Haciendo:**
> Desarrollo de un plugin nativo para validación de formatos y términos clave en contratos bancarios.

**🔗 Conexión con el Proyecto Real:**
Se estudian los 6 plugins implementados — especialmente `CotizacionPlugin.cs` (42KB) con funciones como:
- `buscar_cliente_identidad()` — Búsqueda de cliente por número de identidad
- `guardar_cotizacion()` — Persistencia en base de datos
- `enviar_cotizacion_whatsapp()` — Envío del resultado por WhatsApp

Y `CitaPlugin.cs` con funciones de:
- `listado_medicos_disponibles_fecha()` — Consulta de disponibilidad
- `agendar_cita()` — Agendamiento completo

Los participantes crean un plugin `ContratoPlugin` con funciones para validar cláusulas bancarias.

---

#### 📅 Día 4 (Jueves) — OCR e Inteligencia de Documentos

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | OCR e Inteligencia de Documentos |
| **Subtemas** | Análisis de documentos bancarios y extracción de campos clave mediante modelos pre-entrenados y razonamiento sobre información extraída |
| **Herramientas Microsoft IA** | Azure AI Document Intelligence (OCR) |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Azure AI Document Intelligence: modelos pre-entrenados vs. custom
- Extracción de campos: facturas, recibos, identidades, formularios
- Flujo: documento → OCR → campos estructurados → razonamiento con LLM
- Integración con Blob Storage para almacenamiento de documentos

**Actividad Práctica — Aprender Haciendo:**
> Creación de un modelo de extracción de datos para formularios de préstamos y estados financieros escaneados.

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo el proyecto maneja documentos multimedia recibidos por WhatsApp en `WhatsappController.ExtractMediaInfo()`:
```csharp
case "document":
    mediaId = message.Document?.Id;
    mimeType = message.Document?.MimeType ?? "application/octet-stream";
    fileName = $"{nameWithoutExt}_{uniqueId}{ext}";
    break;
```
Y cómo `BlobService.UploadWhatsAppMediaAsync()` almacena documentos para procesamiento posterior. Los participantes extienden este flujo con Document Intelligence para extraer datos de formularios bancarios automáticamente.

---

#### 📅 Día 5 (Viernes) — Orquestación Inicial y Flujos

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Orquestación Inicial y Flujos |
| **Subtemas** | Uso de historiales de chat, manejo de estado (contextual awareness) y servicios de soporte para persistencia |
| **Herramientas Microsoft IA** | Semantic Kernel (Chat History), Azure Blob Storage |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- `ChatHistory`: acumulación de contexto entre turnos
- Estado conversacional: máquinas de estado por sesión
- Persistencia de conversaciones en base de datos
- Determinación de intención y enrutamiento a flujos especializados

**Actividad Práctica — Aprender Haciendo:**
> Integración del Kernel con Document Intelligence para un flujo básico de análisis de documentos bancarios.

**🔗 Conexión con el Proyecto Real:**
Se analiza `ConversationSession` en `CotizacionIntegration.cs`:
```csharp
// Estado por número de teléfono:
- PhoneNumber → Cliente identificado
- CurrentIntent → cotizacion | cita | domicilio | informacion
- CurrentState → Máquina de estado del flujo activo
- History → Historial de mensajes acumulado
- FrustrationCount → Escalamiento a agente humano
```
Y cómo `WhatsAppChatController.RegistrarMensajeEntrante()` persiste todo en BD. Los participantes diseñan un flujo de onboarding bancario con las mismas técnicas.

---

### SEMANA 2: Memoria y Conocimiento — Del Embedding al RAG

---

#### 📅 Día 6 (Lunes) — Embeddings y Representación Vectorial

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Embeddings y Representación Vectorial |
| **Subtemas** | Conceptos de similaridad semántica, generación de vectores con ADA-002 y transformación de texto legal a representaciones numéricas |
| **Herramientas Microsoft IA** | Azure OpenAI (Embedding models — text-embedding-ada-002) |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- ¿Qué es un embedding? Representación numérica del significado
- Similaridad coseno: cómo se comparan documentos
- Modelos de embeddings: text-embedding-ada-002, text-embedding-3-small/large
- Chunking: cómo dividir documentos largos para vectorización

**Actividad Práctica — Aprender Haciendo:**
> Generación de vectores a partir de normativas legales y políticas de cumplimiento bancario.

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo el archivo `Datos/InfoLaboratorio.txt` actualmente se inyecta como texto completo en el prompt del `LaboratorioPlugin.infoLab()`. Se discute la limitación de este enfoque (consume tokens, no escala) y se introduce embeddings como solución superior. Los participantes vectorizan un corpus de regulaciones bancarias.

---

#### 📅 Día 7 (Martes) — Bases de Datos Vectoriales (Vector Stores)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Bases de Datos Vectoriales (Vector Stores) |
| **Subtemas** | Almacenamiento y recuperación eficiente de información, reducción de consumo de tokens y conectores de memoria |
| **Herramientas Microsoft IA** | Azure AI Search, Semantic Kernel Memory Connectors |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Azure AI Search como vector store: índices, campos vectoriales
- Semantic Kernel Memory Connectors: TextMemoryPlugin
- Reducción de tokens: de inyectar todo el texto → solo los fragmentos relevantes
- Índices de búsqueda: creación, actualización, eliminación

**Actividad Práctica — Aprender Haciendo:**
> Configuración de una base de datos vectorial para indexar normativas bancarias actualizadas.

**🔗 Conexión con el Proyecto Real:**
Se identifica la oportunidad: el agente actual inyecta información de laboratorio como texto plano. Con Azure AI Search, se podría indexar todo el catálogo de análisis, requisitos y precios para búsqueda semántica — reduciendo tokens y mejorando precisión.

---

#### 📅 Día 8 (Miércoles) — Arquitectura RAG (Retrieval-Augmented Generation)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Arquitectura RAG |
| **Subtemas** | Patrón RAG, inyección de contexto en tiempo real y optimización de consultas para mejorar la precisión |
| **Herramientas Microsoft IA** | Semantic Kernel, Azure AI Search, Azure OpenAI |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Patrón RAG: Retrieve → Augment → Generate
- Cuándo usar RAG vs. fine-tuning vs. prompt engineering
- Grounding: anclar las respuestas del LLM a documentos reales
- Reducción de alucinaciones mediante contexto verificable

**Actividad Práctica — Aprender Haciendo:**
> Construcción de un asistente que responda preguntas legales basadas exclusivamente en documentos del Vector Store.

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo el prompt `InfoLaboratorio` actualmente funciona sin RAG (todo el contexto en el prompt). Se implementa la versión RAG: buscar en el índice vectorial → inyectar solo los fragmentos relevantes → generar respuesta. Los participantes aplican lo mismo para normativas bancarias.

---

#### 📅 Día 9 (Jueves) — Búsqueda Textual e Híbrida

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Búsqueda Textual e Híbrida |
| **Subtemas** | Diferencias entre búsqueda textual vs. semántica y configuración de búsqueda híbrida |
| **Herramientas Microsoft IA** | Azure AI Search |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Búsqueda textual (BM25) vs. semántica (vectorial) vs. híbrida
- Cuándo cada tipo es más efectivo: términos técnicos vs. significado
- Configuración de scoring profiles y pesos
- Filtros y facetas para resultados precisos

**Actividad Práctica — Aprender Haciendo:**
> Implementación de un sistema de búsqueda híbrida para mejorar la precisión en la recuperación de normativas.

**🔗 Conexión con el Proyecto Real:**
Se analiza cómo `AnalisisClinicosPlugin.buscarCoincidentes()` actualmente busca por coincidencia de strings. Se demuestra cómo la búsqueda híbrida combinaría match exacto de códigos de análisis (textual) con búsqueda por significado (semántica) para términos como "perfil hepático" → "función del hígado".

---

#### 📅 Día 10 (Viernes) — Automatización y APIs Externas

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Automatización y APIs Externas |
| **Subtemas** | Generación de plugins desde Swagger/OpenAPI y conexión con microservicios o servicios de terceros |
| **Herramientas Microsoft IA** | Semantic Kernel (OpenAPI skills), Power Automate |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Plugins desde OpenAPI/Swagger: `ImportPluginFromOpenApiAsync()`
- Configuración de autenticación para APIs externas
- Power Automate: flujos sin código conectados al agente
- Patrones de integración: síncrono vs. asíncrono

**Actividad Práctica — Aprender Haciendo:**
> Conexión del agente a una API simulada de core bancario para consultar saldos y verificar identidades.

**🔗 Conexión con el Proyecto Real:**
Se estudia cómo los plugins nativos ya se conectan a la API REST de WhatsApp Cloud y al Blob Storage. `CotizacionPlugin.enviar_cotizacion_whatsapp()` llama métodos estáticos de `WhatsappController`. Los participantes crean un plugin OpenAPI para consumir una API bancaria simulada.

---

### SEMANA 3: Multimodalidad y Arquitectura Avanzada

---

#### 📅 Día 11 (Lunes) — Agentes Multimodales: Audio

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Agentes Multimodales: Audio |
| **Subtemas** | Transcripción con Whisper e integración con FFmpeg para extracción de audio |
| **Herramientas Microsoft IA** | Azure OpenAI (Whisper), FFmpeg |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Azure OpenAI Whisper: modelos de transcripción de audio
- Formatos soportados y conversión con FFmpeg
- REST API vs. SDK para audio processing
- Flujo multimodal: audio → texto → agente → respuesta

**Actividad Práctica — Aprender Haciendo:**
> Automatización de la transcripción de llamadas de servicio al cliente para detectar riesgos legales.

**🔗 Conexión con el Proyecto Real (YA IMPLEMENTADO ✅):**
Se estudia directamente `CotizacionIntegration.TranscribeAudio()`:
```csharp
// Configuración real en producción:
Endpoint: "https://agentelabmendez-audio-resource.cognitiveservices.azure.com"
Deployment: "gpt-4o-mini-transcribe"

// Flujo completo:
1. Recibe MediaId del webhook de WhatsApp
2. Descarga audio con Bearer token de WhatsApp API
3. POST multipart/form-data al endpoint de Azure
4. Parsea respuesta JSON → texto transcrito
5. Pasa transcripción a ProcessAgentRequest()
```
Los participantes replican este flujo para transcribir grabaciones de servicio al cliente bancario y analizarlas con el LLM.

---

#### 📅 Día 12 (Martes) — Agentes Multimodales: Visión e Imágenes

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Agentes Multimodales: Visión e Imágenes |
| **Subtemas** | Generación de imágenes (DALL-E 3) y análisis visual de documentos de identidad o garantías |
| **Herramientas Microsoft IA** | Azure OpenAI (DALL-E 3, GPT-4 with Vision), Azure AI Vision |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- GPT-4 Vision: análisis de imágenes con prompts
- DALL-E 3: generación de imágenes descriptivas
- Azure AI Vision: OCR especializado en imágenes
- Casos de uso: KYC, verificación de documentos, análisis de garantías

**Actividad Práctica — Aprender Haciendo:**
> Análisis de fotografías de identificaciones (KYC) y validación de autenticidad de firmas en documentos.

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo `WhatsappController.ExtractMediaInfo()` ya maneja imágenes recibidas por WhatsApp:
```csharp
case "image":
    mediaId = message.Image?.Id;
    caption = message.Image?.Caption;
    mimeType = message.Image?.MimeType ?? "image/jpeg";
```
Y cómo `DomicilioPlugin.guardar_foto_whatsapp()` procesa fotos enviadas por pacientes. Se extiende con GPT-4 Vision para analizar documentos de identidad y extraer datos visualmente.

---

#### 📅 Día 13 (Miércoles) — Arquitectura Multi-Agente y Planners

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Arquitectura Multi-Agente y Planners |
| **Subtemas** | Sistemas de múltiples agentes, delegación de tareas especializadas y uso de Planners para autonomía |
| **Herramientas Microsoft IA** | Semantic Kernel Agent Framework (ChatCompletionAgent), Planners |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- `ChatCompletionAgent`: agentes con personalidad y herramientas propias
- `AgentGroupChat`: colaboración entre múltiples agentes
- Planners: Handlebars Planner y Stepwise Planner
- Delegación: agente coordinador → agentes especializados
- Terminación de conversaciones multi-agente

**Actividad Práctica — Aprender Haciendo:**
> Diseñar una mesa de aprobación de créditos donde un agente líder delega tareas a agentes de cumplimiento.

**🔗 Conexión con el Proyecto Real:**
Se analiza cómo `CotizacionIntegration` ya implementa un **sistema proto-multi-agente** con clasificación de intenciones:
```
Intents: cotizacion → CotizacionPlugin
         agendacitamedica → CitaPlugin
         tomadomicilio → DomicilioPlugin
         informaciongeneral → LaboratorioPlugin
         agentehumano → Transfer a humano
```
Se evoluciona esta arquitectura hacia agentes independientes con `ChatCompletionAgent` para el dominio bancario: AgenteCompliance, AgenteLegal, AgenteCredito.

---

#### 📅 Día 14 (Jueves) — Copilot Studio y Low-Code

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Copilot Studio y Low-Code |
| **Subtemas** | Configuración de agentes inteligentes, diseño de tópicos y creación de copilotos personalizados |
| **Herramientas Microsoft IA** | Microsoft Copilot Studio |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Copilot Studio: creación de agentes sin código
- Tópicos, entidades y variables en Copilot Studio
- Integración con Power Automate para acciones complejas
- Publicación en múltiples canales (Teams, Web, WhatsApp)

**Actividad Práctica — Aprender Haciendo:**
> Creación y publicación de un agente conversacional integrado con flujos de Power Automate.

**🔗 Conexión con el Proyecto Real:**
Se compara el enfoque **code-first** del proyecto (Semantic Kernel + C#) con el enfoque **low-code** de Copilot Studio. Se discuten los trade-offs: flexibilidad vs. velocidad de desarrollo. Se crea un copiloto de consultas frecuentes que complementa al agente de código.

---

#### 📅 Día 15 (Viernes) — Razonamiento y Toma de Decisiones

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Razonamiento y Toma de Decisiones |
| **Subtemas** | Apoyo a la decisión en entornos regulados y reducción de alucinaciones |
| **Herramientas Microsoft IA** | Azure OpenAI (GPT-4 reasoning), Semantic Kernel |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Cadenas de razonamiento: chain-of-thought, tree-of-thought
- Reducción de alucinaciones: grounding, restricciones, validación
- Modelos de razonamiento: o1, o3 vs. GPT-4
- Lógica de decisión: scoring, ponderación, reglas de negocio

**Actividad Práctica — Aprender Haciendo:**
> Implementación de lógica de recomendación de crédito basada en el riesgo analizado por la IA.

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo el system prompt del agente incluye **PROHIBICIONES** explícitas para reducir alucinaciones:
```
"PROHIBICIONES:
- NUNCA interpretes resultados de análisis
- NUNCA des diagnósticos médicos
- NUNCA des recomendaciones de tratamiento"
```
Y cómo `FrustrationCount` en `ConversationSession` implementa escalamiento automático cuando el agente no puede resolver. Los participantes implementan lógica de scoring de riesgo crediticio con guardrails.

---

### SEMANA 4: Producción, Seguridad y Proyecto Final

---

#### 📅 Día 16 (Lunes) — Framework de Procesos y Onboarding

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Framework de Procesos y Onboarding |
| **Subtemas** | Encadenamiento de pasos (Steps) y eventos en flujos de trabajo de IA para procesos complejos |
| **Herramientas Microsoft IA** | Semantic Kernel Process Framework |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Semantic Kernel Process Framework: Steps, Events, State
- Modelado de procesos complejos con múltiples pasos
- Eventos de entrada/salida entre Steps
- Persistencia de estado del proceso

**Actividad Práctica — Aprender Haciendo:**
> Modelado de un proceso de onboarding de clientes con múltiples pasos de validación automática.

**🔗 Conexión con el Proyecto Real:**
Se analiza el flujo de **Toma a Domicilio** implementado en `DomicilioPlugin.cs`:
```
Paso 1: validar_fecha_disponible()
Paso 2: validar_hora_disponible()
Paso 3: solicitar_ubicacion() → guardar_ubicacion()
Paso 4: solicitar_foto_casa() → guardar_foto_whatsapp()
Paso 5: agendar_toma_domicilio()
Paso 6: enviar_recordatorio_toma()
```
Este flujo multi-paso se modela formalmente con Process Framework para onboarding bancario: KYC → Verificación → Scoring → Aprobación.

---

#### 📅 Día 17 (Martes) — Seguridad, Ética y Gobernanza

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Seguridad, Ética y Gobernanza |
| **Subtemas** | Filtros de contenido, manejo de PII y prevención de inyección de prompts |
| **Herramientas Microsoft IA** | Azure AI Content Safety, Microsoft Purview, Entra ID |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Azure AI Content Safety: detección de contenido dañino
- PII (Información Personal Identificable): detección y redacción
- Prompt Injection: tipos de ataques y mitigaciones
- Entra ID: autenticación y autorización
- Responsible AI: principios de Microsoft

**Actividad Práctica — Aprender Haciendo:**
> Configuración de filtros de seguridad para evitar fuga de información bancaria sensible.

**🔗 Conexión con el Proyecto Real:**
Se audita la seguridad actual del proyecto:
- ✅ JWT Authentication con `Bearer` tokens
- ✅ Tenant filtering con `EmpresaId` (query filters de EF Core)
- ✅ Soft delete con `Modificacion == 3`
- ⚠️ Sin Content Safety filters
- ⚠️ Sin detección de PII
- ⚠️ Sin prevención de prompt injection

Los participantes implementan las capas faltantes para el contexto bancario, donde la seguridad es crítica.

---

#### 📅 Día 18 (Miércoles) — Observabilidad y Evaluación (MLOps)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Observabilidad y Evaluación (MLOps) |
| **Subtemas** | Evaluación de performance, monitoreo de tasa de alucinaciones, telemetría y trazabilidad |
| **Herramientas Microsoft IA** | Azure Monitor, Application Insights, Foundry Control Plane |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Application Insights: telemetría de llamadas al LLM
- Métricas clave: latencia, tokens consumidos, tasa de éxito
- Evaluación de calidad: groundedness, relevance, coherence
- Trazabilidad: logging de cada paso del agente

**Actividad Práctica — Aprender Haciendo:**
> Realización de pruebas de estrés y trazado del razonamiento del agente para auditoría interna.

**🔗 Conexión con el Proyecto Real:**
Se muestra el sistema de logging implementado:
- `IWhatsAppLoggingService` — logging centralizado de mensajes
- `WhatsAppBaseController` — controller base con logging
- `LogMessageToDatabase()` — persistencia de cada mensaje en BD
- `ILogger<WhatsappController>` — logging estructurado con Microsoft.Extensions.Logging

Los participantes agregan Application Insights y crean dashboards de monitoreo.

---

#### 📅 Día 19 (Jueves) — Visualización y Analítica de Resultados

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Visualización y Analítica de Resultados |
| **Subtemas** | Monitoreo del desempeño del agente, precisión de extracción OCR y métricas de negocio |
| **Herramientas Microsoft IA** | Power BI, Power Platform, Fabric IQ |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Power BI: conexión a datos del agente
- Métricas de negocio: conversiones, tiempos de respuesta, satisfacción
- Dashboards en tiempo real con DirectQuery
- Fabric IQ: análisis avanzado de datasets de IA

**Actividad Práctica — Aprender Haciendo:**
> Diseño de un dashboard en Power BI que muestre estadísticas de documentos aprobados y errores detectados.

**🔗 Conexión con el Proyecto Real:**
Se conecta Power BI a las tablas del proyecto:
- `MgWhatsappConversacion` — conversaciones y estados
- `MgWhatsappMensaje` — mensajes con tipos, timestamps
- `MgCotizacion` — cotizaciones generadas por el agente
- `MgCitaMedica` — citas agendadas por el agente
- `MgTomaDomicilio` — tomas programadas

Los participantes crean un dashboard equivalente para métricas bancarias.

---

#### 📅 Día 20 (Viernes) — Proyecto Final: Solución Bancaria Integral

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Proyecto Final: Solución Bancaria Integral |
| **Subtemas** | Validación de la solución multidisciplinaria (OCR + RAG + Agentes) bajo el enfoque "aprender haciendo" |
| **Herramientas Microsoft IA** | Ecosistema Microsoft IA (Azure OpenAI, Copilot Studio, Azure AI Studio) |
| **Duración** | ~2 horas |

**Contenido Teórico:**
- Revisión arquitectónica de la solución completa
- Patrones de despliegue: App Service, Container Apps
- Checklist de producción: seguridad, observabilidad, escalabilidad

**Actividad Práctica — Aprender Haciendo:**
> Presentación de la solución funcional capaz de analizar, razonar y decidir sobre un caso bancario real.

**🔗 Conexión con el Proyecto Real:**
Se presenta el proyecto completo como ejemplo de una solución ya desplegada en producción:
- Despliegue en Azure App Service (ver `labmendez-api.service`)
- `dotnet publish` → Blob/Server deployment
- Configuración por ambiente: `appsettings.{Development|Staging|Production}.json`
- Background jobs con Hangfire

Los participantes consolidan su proyecto final: agente bancario con OCR + RAG + Multi-Agente + Seguridad + Monitoreo.

---

## Resumen de Tecnologías por Semana

| Semana | Enfoque | Tecnologías Clave | Referencia en Proyecto |
|--------|---------|-------------------|------------------------|
| **1** | Fundamentos | Semantic Kernel, Azure OpenAI, Plugins nativos, Prompt Templates | `Program.cs`, `Plugins/*.cs`, `Prompts/` |
| **2** | Memoria/RAG | Embeddings, Azure AI Search, RAG, OpenAPI plugins | `LaboratorioPlugin`, `AnalisisClinicosPlugin` |
| **3** | Multimodalidad | Whisper, GPT-4 Vision, DALL-E, Multi-Agente, Copilot Studio | `CotizacionIntegration.TranscribeAudio()`, `ExtractMediaInfo()` |
| **4** | Producción | Process Framework, Content Safety, Application Insights, Power BI | `DomicilioPlugin`, logging, deployment configs |

## Mapeo: Jornalización ↔ Proyecto Implementado

| Tema del Curso | Estado en el Proyecto | Archivo(s) |
|----------------|----------------------|------------|
| Configuración del Kernel | ✅ Implementado | `Program.cs` |
| Prompt Templates | ✅ Implementado | `Prompts/LabAnalisis/` |
| Plugins Nativos C# | ✅ Implementado (6 plugins) | `Plugins/*.cs` |
| Function Calling / Auto-Invoke | ✅ Implementado | `CotizacionIntegration.cs` |
| Chat History / Estado | ✅ Implementado | `ConversationSession` |
| Audio Transcription (Whisper) | ✅ Implementado | `CotizacionIntegration.TranscribeAudio()` |
| Manejo de imágenes multimedia | ✅ Implementado | `WhatsappController.ExtractMediaInfo()` |
| Blob Storage | ✅ Implementado | `BlobService.cs` |
| Multi-canal (WhatsApp/Messenger/Web) | ✅ Implementado | Controllers de WhatsApp |
| Multi-LLM (OpenAI/Gemini) | ✅ Implementado | `Program.cs`, `CotizacionIntegration.cs` |
| Clasificación de intenciones | ✅ Implementado | `DeterminarIntencion` prompt |
| OCR / Document Intelligence | 🔲 No implementado | — |
| Embeddings / Vector Store | 🔲 No implementado | — |
| RAG (Retrieval-Augmented Generation) | 🔲 No implementado | — |
| Búsqueda híbrida | 🔲 No implementado | — |
| GPT-4 Vision | 🔲 No implementado | — |
| Multi-Agente formal | 🔲 No implementado (proto-multi-agente) | — |
| Copilot Studio | 🔲 No implementado | — |
| Content Safety | 🔲 No implementado | — |
| Application Insights | 🔲 No implementado | — |
| Process Framework | 🔲 No implementado (flujo manual en plugin) | — |

---

## Valor Diferenciador del Curso

> **"No es un curso teórico más."** Cada tema se ancla a un sistema **real en producción** que procesa mensajes de WhatsApp, transcribe audios, genera cotizaciones y agenda citas — todo orquestado por Semantic Kernel con Azure OpenAI. Los participantes ven código real, entienden decisiones de arquitectura reales, y construyen su propia solución bancaria con las mismas herramientas.
