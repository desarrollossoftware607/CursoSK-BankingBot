# ============================================================
# Sesión 2: Crear deployment de Whisper (Audio Transcription)
# ============================================================
# Prerequisitos: Haber ejecutado 01-crear-recurso-openai.ps1
#
# PASOS EQUIVALENTES EN EL PORTAL DE AZURE:
# 1. Ir a https://portal.azure.com → tu recurso Azure OpenAI
# 2. "Model deployments" → "Manage Deployments"
# 3. Clic "+ Create new deployment"
#    - Model: gpt-4o-mini-audio-preview (o whisper)
#    - Deployment name: gpt-4o-mini-transcribe
#    - Clic "Create"
# 4. Copiar el nombre del deployment a appsettings.Development.json
#    sección LLMSettings:Audio:DeploymentName
# ============================================================

. "$PSScriptRoot\00-variables.ps1"
Test-AzCli
Ensure-AzLogin

Write-Host "`n🎙️ Creando deployment de audio ($AUDIO_DEPLOYMENT)..." -ForegroundColor Yellow
az cognitiveservices account deployment create `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --deployment-name $AUDIO_DEPLOYMENT `
    --model-name $AUDIO_MODEL `
    --model-format OpenAI `
    --sku-name GlobalStandard `
    --sku-capacity 1 `
    --output table

Write-Host "`n✅ Deployment de audio creado: $AUDIO_DEPLOYMENT" -ForegroundColor Green
Write-Host "📋 Agregar a appsettings.Development.json en la sección LLMSettings:Audio" -ForegroundColor Cyan
