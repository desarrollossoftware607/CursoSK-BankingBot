# ============================================================
# Sesión 1: Crear Resource Group + Azure OpenAI + Chat Deployment
# ============================================================
# Prerequisitos: Azure CLI instalado, sesión iniciada (az login)
#
# PASOS EQUIVALENTES EN EL PORTAL DE AZURE:
# 1. Ir a https://portal.azure.com
# 2. Buscar "Resource groups" → "+ Create"
#    - Subscription: seleccionar tu suscripción
#    - Resource group: rg-curso-semantic-kernel
#    - Region: East US 2
#    - Clic "Review + Create" → "Create"
# 3. Buscar "Azure OpenAI" → "+ Create"
#    - Resource group: rg-curso-semantic-kernel
#    - Region: East US 2
#    - Name: oai-curso-sk
#    - Pricing tier: Standard S0
#    - Clic "Review + Create" → "Create"
# 4. Ir al recurso → "Model deployments" → "Manage Deployments"
#    - Clic "+ Create new deployment"
#    - Model: gpt-4o-mini
#    - Deployment name: gpt-4o-mini
#    - Clic "Create"
# 5. Ir al recurso → "Keys and Endpoint"
#    - Copiar Key 1 y Endpoint
#    - Pegarlos en appsettings.Development.json
# ============================================================

. "$PSScriptRoot\00-variables.ps1"
Test-AzCli
Ensure-AzLogin

Write-Host "`n📁 Paso 1: Crear Resource Group..." -ForegroundColor Yellow
az group create `
    --name $RESOURCE_GROUP `
    --location $LOCATION `
    --output table

Write-Host "`n🤖 Paso 2: Crear recurso Azure OpenAI..." -ForegroundColor Yellow
az cognitiveservices account create `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --location $LOCATION `
    --kind OpenAI `
    --sku S0 `
    --output table

Write-Host "`n🚀 Paso 3: Crear deployment de chat ($CHAT_DEPLOYMENT)..." -ForegroundColor Yellow
az cognitiveservices account deployment create `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --deployment-name $CHAT_DEPLOYMENT `
    --model-name $CHAT_MODEL `
    --model-version $CHAT_MODEL_VERSION `
    --model-format OpenAI `
    --sku-name Standard `
    --sku-capacity 10 `
    --output table

Write-Host "`n🔑 Paso 4: Obtener credenciales..." -ForegroundColor Yellow
$KEY = az cognitiveservices account keys list `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --query "key1" -o tsv
$ENDPOINT = az cognitiveservices account show `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --query "properties.endpoint" -o tsv

Write-Host "`n✅ Recurso creado exitosamente!" -ForegroundColor Green
Write-Host "   Endpoint: $ENDPOINT"
Write-Host "   API Key:  $($KEY.Substring(0,8))..."
Write-Host "`n📋 Copiar en appsettings.Development.json:" -ForegroundColor Cyan
Write-Host @"
{
  "LLMSettings": {
    "AzureOpenAI": {
      "DeploymentName": "$CHAT_DEPLOYMENT",
      "Endpoint": "$ENDPOINT",
      "ApiKey": "$KEY"
    }
  }
}
"@
