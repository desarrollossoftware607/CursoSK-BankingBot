namespace CursoSK.Api.DTOs;

public record PromptRequest(string Prompt);
public record PromptConSettingsRequest(string Prompt, int MaxTokens = 200, double Temperature = 0.7);
public record BlogRequest(string Tema);
public record ChatMensajeRequest(string Mensaje);
public record ChatImagenRequest(string ImagenUrl, string? Pregunta);
