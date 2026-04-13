# ============================================================
# Sesión 10: Deploy de la API a Azure App Service
# ============================================================
# Prerequisitos: Resource Group + Azure OpenAI creados
#                Proyecto compilado y funcionando localmente
#
# PASOS EQUIVALENTES EN EL PORTAL DE AZURE:
# 1. Ir a https://portal.azure.com
# 2. Buscar "App Services" → "+ Create" → "Web App"
#    - Resource group: rg-curso-semantic-kernel
#    - Name: app-curso-sk-XXXX (nombre único global)
#    - Runtime stack: .NET 9 (STS)
#    - Region: East US 2
#    - Pricing plan: Basic B1 (~$13/mes)
#    - Clic "Review + Create" → "Create"
# 3. Ir al recurso → "Configuration" → "Application settings"
#    - Agregar las variables de entorno:
#      LLMSettings__AzureOpenAI__DeploymentName = gpt-4o-mini
#      LLMSettings__AzureOpenAI__Endpoint = https://...
#      LLMSettings__AzureOpenAI__ApiKey = ...
#      LLMSettings__Embedding__DeploymentName = text-embedding-3-small
#      LLMSettings__Embedding__Endpoint = https://...
#      LLMSettings__Embedding__ApiKey = ...
#    - Guardar
# 4. Ir a "Deployment Center"
#    - Source: Local Git o GitHub Actions
#    - Configurar y hacer push
# 5. Abrir la URL del App Service → /swagger
#
# ALTERNATIVA RÁPIDA (CLI):
#   Publicar con `az webapp up` sin crear plan manualmente
# ============================================================

. "$PSScriptRoot\00-variables.ps1"
Test-AzCli
Ensure-AzLogin

# --- Seleccionar qué proyecto deploy ---
Write-Host "`n¿Qué proyecto desea desplegar?" -ForegroundColor Yellow
Write-Host "  1) CursoSK.Api (genérico)"
Write-Host "  2) CursoSK.BankingBot (bancario)"
$choice = Read-Host "Seleccione (1 o 2)"
$PROJECT = if ($choice -eq "2") { "CursoSK.BankingBot" } else { "CursoSK.Api" }
$PROJECT_PATH = Join-Path $PSScriptRoot "..\..\$PROJECT"

Write-Host "`n📦 Paso 1: Publicar proyecto $PROJECT..." -ForegroundColor Yellow
Push-Location $PROJECT_PATH
dotnet clean
dotnet publish -c Release -o ./publish_output
Pop-Location

Write-Host "`n🏗️ Paso 2: Crear App Service Plan..." -ForegroundColor Yellow
az appservice plan create `
    --name $APP_SERVICE_PLAN `
    --resource-group $RESOURCE_GROUP `
    --location $LOCATION `
    --sku $APP_SERVICE_SKU `
    --is-linux `
    --output table

Write-Host "`n🌐 Paso 3: Crear Web App..." -ForegroundColor Yellow
az webapp create `
    --name $APP_SERVICE_NAME `
    --resource-group $RESOURCE_GROUP `
    --plan $APP_SERVICE_PLAN `
    --runtime "DOTNETCORE:9.0" `
    --output table

Write-Host "`n⚙️ Paso 4: Configurar variables de entorno..." -ForegroundColor Yellow
# El usuario debe ajustar estos valores con sus credenciales reales
$OPENAI_ENDPOINT = az cognitiveservices account show `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --query "properties.endpoint" -o tsv
$OPENAI_KEY = az cognitiveservices account keys list `
    --name $OPENAI_NAME `
    --resource-group $RESOURCE_GROUP `
    --query "key1" -o tsv

az webapp config appsettings set `
    --name $APP_SERVICE_NAME `
    --resource-group $RESOURCE_GROUP `
    --settings `
        "LLMSettings__Provider=azure" `
        "LLMSettings__AzureOpenAI__DeploymentName=$CHAT_DEPLOYMENT" `
        "LLMSettings__AzureOpenAI__Endpoint=$OPENAI_ENDPOINT" `
        "LLMSettings__AzureOpenAI__ApiKey=$OPENAI_KEY" `
    --output table

Write-Host "`n🚀 Paso 5: Desplegar código..." -ForegroundColor Yellow
$PUBLISH_PATH = Join-Path $PROJECT_PATH "publish_output"
$ZIP_PATH = Join-Path $PSScriptRoot "deploy.zip"
Compress-Archive -Path "$PUBLISH_PATH\*" -DestinationPath $ZIP_PATH -Force
az webapp deploy `
    --name $APP_SERVICE_NAME `
    --resource-group $RESOURCE_GROUP `
    --src-path $ZIP_PATH `
    --type zip

$APP_URL = "https://$APP_SERVICE_NAME.azurewebsites.net"
Write-Host "`n✅ Desplegado exitosamente!" -ForegroundColor Green
Write-Host "   URL: $APP_URL"
Write-Host "   Swagger: $APP_URL/swagger"
Write-Host "`n🔗 Abrir en navegador:" -ForegroundColor Cyan
Start-Process $APP_URL/swagger

# Limpieza
Remove-Item $ZIP_PATH -ErrorAction SilentlyContinue
