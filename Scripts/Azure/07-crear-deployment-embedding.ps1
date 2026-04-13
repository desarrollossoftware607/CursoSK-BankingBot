# ============================================================
# Sesión 7: Crear deployment de Embedding
# ============================================================
# Prerequisitos: Haber ejecutado 01-crear-recurso-openai.ps1
#
# PASOS EQUIVALENTES EN EL PORTAL DE AZURE:
# 1. Ir a https://portal.azure.com → tu recurso Azure OpenAI
# 2. "Model deployments" → "Manage Deployments"
# 3. Clic "+ Create new deployment"
#    - Model: text-embedding-3-small
#    - Deployment name: text-embedding-3-small
#    - Clic "Create"
# 4. Copiar el nombre del deployment a appsettings.Development.json
#    sección LLMSettings:Embedding
#
# NOTA: Si tu recurso Azure OpenAI no tiene el modelo 
#       text-embedding-3-small disponible, puedes crear un 
#       recurso separado en una región que lo soporte (ej: eastus).
#       En ese caso, copia el Endpoint y Key del NUEVO recurso 
#       a la sección Embedding del appsettings.
# ============================================================

. "$PSScriptRoot\00-variables.ps1"
Test-AzCli
Ensure-AzLogin

Write-Host "`n📐 Creando deployment de embedding ($EMBEDDING_DEPLOYMENT)..." -ForegroundColor Yellow
az cognitiveservices account deployment create `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --deployment-name $EMBEDDING_DEPLOYMENT `
    --model-name $EMBEDDING_MODEL `
    --model-format OpenAI `
    --sku-name Standard `
    --sku-capacity 10 `
    --output table

$ENDPOINT = az cognitiveservices account show `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --query "properties.endpoint" -o tsv
$KEY = az cognitiveservices account keys list `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --query "key1" -o tsv

Write-Host "`n✅ Deployment de embedding creado: $EMBEDDING_DEPLOYMENT" -ForegroundColor Green
Write-Host "📋 Agregar a appsettings.Development.json:" -ForegroundColor Cyan
Write-Host @"
"Embedding": {
  "DeploymentName": "$EMBEDDING_DEPLOYMENT",
  "Endpoint": "$ENDPOINT",
  "ApiKey": "$KEY"
}
"@
