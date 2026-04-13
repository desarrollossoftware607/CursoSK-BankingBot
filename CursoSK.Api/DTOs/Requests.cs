namespace CursoSK.Api.DTOs;

public record PromptRequest(string Prompt);
public record PromptConSettingsRequest(string Prompt, int MaxTokens = 200, double Temperature = 0.7);
