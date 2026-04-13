# 📚 Corpus Legal Bancario — Honduras

Archivos de texto plano con las leyes y normativas bancarias hondureñas
utilizadas como fuente de conocimiento para el sistema RAG (Retrieval-Augmented Generation).

## Archivos

| Archivo | Ley | Categoría |
|---------|-----|-----------|
| `Decreto_129_2004_Ley_Sistema_Financiero.txt` | Decreto 129-2004 | Bancaria |
| `Decreto_144_2014_Ley_Lavado_Activos.txt` | Decreto 144-2014 | Lavado de Activos |
| `Decreto_170_2016_Proteccion_Consumidor_Financiero.txt` | Decreto 170-2016 | Protección al Consumidor |
| `Resolucion_CNBS_GES_041_2019_Riesgo_Crediticio.txt` | Resolución CNBS-GES-041-2019 | Gestión de Riesgo |

## Flujo RAG

```
1. Cargar archivo .txt → Chunking (dividir en fragmentos)
2. Cada fragmento → Embedding (text-embedding-ada-002)
3. Embedding → Vector Store (InMemory / Azure AI Search)
4. Consulta del usuario → Embedding de la consulta
5. Búsqueda semántica → Top K fragmentos relevantes
6. Fragmentos + Pregunta → LLM → Respuesta fundamentada
```

## Endpoint de carga

```
POST /api/rag/indexar/leyes
```

Carga automáticamente todos los archivos `.txt` de esta carpeta,
los divide en chunks y los indexa en el vector store.

## Uso en clase

Estos archivos se usan en las **clases de la Semana 2** (Días 6-9)
para demostrar:
- Generación de embeddings
- Bases de datos vectoriales
- Arquitectura RAG
- Búsqueda híbrida (textual + semántica)
