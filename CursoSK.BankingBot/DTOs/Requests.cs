namespace CursoSK.BankingBot.DTOs;

// ─── Onboarding & Préstamos ────────────────────────────────
public record CrearClienteRequest(string Identidad, string NombreCompleto, string Correo, string Telefono, string Direccion, DateTime FechaNacimiento, string Ocupacion, decimal IngresoMensual);

public record SolicitarPrestamoRequest(string Identidad, string TipoPrestamo, decimal MontoSolicitado, int PlazoMeses);

public record SimulacionRequest(decimal Monto, int PlazoMeses, decimal TasaAnual);

// ─── Chat ──────────────────────────────────────────────────
public record ChatMensajeRequest(string Mensaje);

// ─── Legal ─────────────────────────────────────────────────
public record ConsultaLegalRequest(string Consulta);
public record AgregarLeyRequest(string Titulo, string? NumeroLey, string Categoria, string Contenido, DateTime FechaPublicacion);

// ─── RAG ───────────────────────────────────────────────────
public record IndexarDocumentoRequest(string Titulo, string Contenido, string Categoria, string? Fuente);
public record BusquedaSemanticaRequest(string Consulta, int Top = 3);
public record RagPreguntaRequest(string Pregunta);

// ─── Documentos ────────────────────────────────────────────
public record AnalisisDocumentoRequest(string Identidad, string TipoDocumento, string ContenidoTexto);

// ─── Kernel ────────────────────────────────────────────────
public record PromptRequest(string Prompt);
