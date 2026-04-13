# ============================================================
# Variables compartidas para todos los scripts de Azure
# Ajusta estos valores según tu suscripción y preferencias
# ============================================================

# --- Suscripción y Grupo de Recursos ---
$SUBSCRIPTION_ID    = "TU-SUBSCRIPTION-ID"
$RESOURCE_GROUP     = "rg-curso-semantic-kernel"
$LOCATION           = "eastus2"

# --- Azure OpenAI ---
$OPENAI_NAME        = "oai-curso-sk"
$CHAT_DEPLOYMENT    = "gpt-4o-mini"
$CHAT_MODEL         = "gpt-4o-mini"
$CHAT_MODEL_VERSION = "2024-07-18"

# --- Whisper / Audio ---
$AUDIO_DEPLOYMENT   = "gpt-4o-mini-transcribe"
$AUDIO_MODEL        = "gpt-4o-mini-audio-preview"

# --- Embedding ---
$EMBEDDING_DEPLOYMENT = "text-embedding-3-small"
$EMBEDDING_MODEL      = "text-embedding-3-small"

# --- Azure AI Search (opcional, Sesión 9) ---
$SEARCH_NAME        = "search-curso-sk"
$SEARCH_SKU         = "free"

# --- App Service (Sesión 10) ---
$APP_SERVICE_PLAN   = "plan-curso-sk"
$APP_SERVICE_NAME   = "app-curso-sk-$(Get-Random -Minimum 1000 -Maximum 9999)"
$APP_SERVICE_SKU    = "B1"

# --- Verificar Azure CLI ---
function Test-AzCli {
    if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
        Write-Host "❌ Azure CLI no encontrado. Instalar desde: https://aka.ms/installazurecli" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ Azure CLI encontrado: $(az version --query '""azure-cli""' -o tsv)" -ForegroundColor Green
}

# --- Login si necesario ---
function Ensure-AzLogin {
    $account = az account show 2>$null | ConvertFrom-Json
    if (-not $account) {
        Write-Host "🔑 Iniciando sesión en Azure..." -ForegroundColor Yellow
        az login
    }
    if ($SUBSCRIPTION_ID -ne "TU-SUBSCRIPTION-ID") {
        az account set --subscription $SUBSCRIPTION_ID
    }
    Write-Host "📋 Suscripción activa: $(az account show --query name -o tsv)" -ForegroundColor Cyan
}

Write-Host "📦 Variables cargadas. Ejecute '. .\00-variables.ps1' para importarlas." -ForegroundColor Green
