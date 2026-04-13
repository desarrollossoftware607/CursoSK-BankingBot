# ============================================================
# Sesión 9: Crear Azure AI Search (opcional para RAG avanzado)
# ============================================================
# Prerequisitos: Resource Group creado (01-crear-recurso-openai.ps1)
#
# PASOS EQUIVALENTES EN EL PORTAL DE AZURE:
# 1. Ir a https://portal.azure.com
# 2. Buscar "AI Search" → "+ Create"
#    - Resource group: rg-curso-semantic-kernel
#    - Service name: search-curso-sk
#    - Location: East US 2
#    - Pricing tier: Free (para demos) o Basic
#    - Clic "Review + Create" → "Create"
# 3. Ir al recurso → "Keys"
#    - Copiar la "Primary admin key"
# 4. Ir a "Overview" → copiar la URL del servicio
# 5. Usar estos valores en la configuración de Azure AI Search
#    connector si se desea RAG con Azure AI Search en vez de
#    vector store en memoria.
#
# NOTA: El tier Free permite 1 índice y 50MB de datos.
#       Suficiente para la demo del curso.
#       Para producción usar Basic ($75/mes) o Standard.
# ============================================================

. "$PSScriptRoot\00-variables.ps1"
Test-AzCli
Ensure-AzLogin

Write-Host "`n🔍 Creando Azure AI Search ($SEARCH_NAME)..." -ForegroundColor Yellow
az search service create `
    --name $SEARCH_NAME `
    --resource-group $RESOURCE_GROUP `
    --location $LOCATION `
    --sku $SEARCH_SKU `
    --output table

$SEARCH_KEY = az search admin-key show `
    --service-name $SEARCH_NAME `
    --resource-group $RESOURCE_GROUP `
    --query "primaryKey" -o tsv
$SEARCH_URL = "https://$SEARCH_NAME.search.windows.net"

Write-Host "`n✅ Azure AI Search creado!" -ForegroundColor Green
Write-Host "   URL:     $SEARCH_URL"
Write-Host "   API Key: $($SEARCH_KEY.Substring(0,8))..."
Write-Host "`n📋 Estos valores son opcionales — el curso usa vector store en memoria por defecto." -ForegroundColor Cyan
