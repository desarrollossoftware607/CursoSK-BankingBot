# ============================================================
# Sesión 10: Configuración de Microsoft Foundry
# ============================================================
# Este script documenta los pasos MANUALES en el portal de
# Microsoft Foundry (ai.azure.com) ya que no todos los pasos
# están disponibles via CLI.
#
# PASOS EN EL PORTAL DE MICROSOFT FOUNDRY:
#
# === PARTE 1: Acceder al Portal ===
# 1. Ir a https://ai.azure.com
# 2. Iniciar sesión con tu cuenta de Azure
# 3. Seleccionar tu recurso Azure OpenAI existente
#    (creado en el script 01)
#
# === PARTE 2: Probar el Playground ===
# 1. En el menú lateral → "Playgrounds" → "Chat"
# 2. Seleccionar tu deployment (gpt-4o-mini)
# 3. En "System message" escribir:
#    "Eres un asistente legal especializado en regulaciones
#     bancarias de Honduras. Responde citando artículos y leyes."
# 4. Probar con: "¿Cuáles son los requisitos para abrir un banco?"
#
# === PARTE 3: Subir Archivos (RAG desde el Portal) ===
# 1. En el Playground → Clic en "Adjuntar archivos" (📎)
# 2. Seleccionar "Crear un nuevo índice vectorial"
#    - Nombre del índice: "leyes-bancarias-hn"
# 3. Arrastrar los archivos de Docs/Leyes/:
#    - Decreto_129_2004_Ley_Sistema_Financiero.txt
#    - Decreto_144_2014_Ley_Lavado_Activos.txt
#    - Decreto_170_2016_Proteccion_Consumidor_Financiero.txt
#    - Resolucion_CNBS_GES_041_2019_Riesgo_Crediticio.txt
# 4. Clic "Adjuntar" → Esperar procesamiento
#    (Foundry: extracción → chunking → embedding → indexación)
# 5. Probar con preguntas legales y observar las citaciones
#
# === PARTE 4: Guardrails (Content Safety) ===
# 1. En el menú → "Safety + security" → "Content filters"
# 2. Observar los filtros por defecto:
#    - Microsoft.Default: Odio, Autolesiones, Sexual, Violencia
#    - Nivel de bloqueo: Medium
#    - Filtro de inyección de prompts: habilitado
# 3. Opcionalmente crear un filtro personalizado
#    - Clic "+ Create content filter"
#    - Ajustar niveles según necesidad
#    - Asignar al deployment del modelo
#
# === PARTE 5: Servicio de Agentes de Foundry ===
# 1. En el menú → "Agents"
# 2. Se muestran 3 tipos de agentes:
#    a) Agentes de Solicitud — Sin código, desde el portal
#    b) Agentes de Flujo de Trabajo — YAML, multi-agente
#    c) Agentes Hospedados — Contenedor, SK/LangGraph
# 3. Para demo: Crear un "Agente de Solicitud"
#    - Nombre: "Asistente Legal Bancario"
#    - Instrucciones del sistema (copiar el system message anterior)
#    - Adjuntar el índice "leyes-bancarias-hn" creado en Parte 3
#    - Probar en el playground del agente
#
# === COMPARATIVA: RAG Portal vs RAG Código ===
# Portal Foundry:
#   ✅ Sin código, rápido de configurar
#   ✅ Indexación automática de archivos
#   ❌ Menos control sobre chunking y prompts
#   ❌ Requiere Azure AI Search (costo adicional)
#
# Código (CursoSK.BankingBot):
#   ✅ Control total sobre chunking, overlap, prompts
#   ✅ Vector store en memoria (sin costo adicional)
#   ✅ Personalizable y testeable
#   ❌ Requiere desarrollo y mantenimiento
# ============================================================

Write-Host "📖 Este script contiene documentación paso a paso." -ForegroundColor Yellow
Write-Host "   No ejecuta comandos — los pasos son manuales en https://ai.azure.com" -ForegroundColor Yellow
Write-Host ""
Write-Host "📋 Pasos incluidos:" -ForegroundColor Cyan
Write-Host "   1. Acceder al portal de Microsoft Foundry"
Write-Host "   2. Probar el Playground de Chat"
Write-Host "   3. Subir archivos para RAG (índice vectorial)"
Write-Host "   4. Configurar Guardrails (Content Safety)"
Write-Host "   5. Crear un Agente de Solicitud"
Write-Host ""
Write-Host "🔗 Portal: https://ai.azure.com" -ForegroundColor Green

# --- Opcionalmente abrir el portal ---
$open = Read-Host "¿Abrir el portal de Foundry en el navegador? (s/n)"
if ($open -eq "s") { Start-Process "https://ai.azure.com" }
