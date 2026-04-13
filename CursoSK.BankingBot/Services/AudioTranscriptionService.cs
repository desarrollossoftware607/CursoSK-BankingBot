using System.Net.Http.Headers;
using System.Text.Json;

namespace CursoSK.BankingBot.Services;

/// <summary>Servicio de transcripción de audio usando Azure OpenAI Whisper.</summary>
public class AudioTranscriptionService
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;
    private readonly string _deploymentName;
    private readonly string _apiKey;
    private readonly ILogger<AudioTranscriptionService> _logger;

    public AudioTranscriptionService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<AudioTranscriptionService> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _endpoint = configuration["LLM:Audio:Endpoint"] ?? configuration["LLM:Azure:Endpoint"]!;
        _deploymentName = configuration["LLM:Audio:DeploymentName"] ?? "gpt-4o-mini-transcribe";
        _apiKey = configuration["LLM:Audio:ApiKey"] ?? configuration["LLM:Azure:ApiKey"]!;
        _logger = logger;
    }

    /// <summary>Transcribe un archivo de audio a texto usando Azure OpenAI Whisper.</summary>
    public async Task<TranscriptionResult> TranscribeAsync(Stream audioStream, string fileName)
    {
        var url = $"{_endpoint.TrimEnd('/')}/openai/deployments/{_deploymentName}/audio/transcriptions?api-version=2024-06-01";

        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(audioStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(GetMimeType(fileName));
        content.Add(streamContent, "file", fileName);
        content.Add(new StringContent("json"), "response_format");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

        _logger.LogInformation("Transcribiendo audio: {FileName}, Deployment: {Deployment}", fileName, _deploymentName);

        var response = await _httpClient.PostAsync(url, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Error en transcripción: {StatusCode} - {Body}", response.StatusCode, responseBody);
            return new TranscriptionResult { Exito = false, Error = $"Error {response.StatusCode}: {responseBody}" };
        }

        var json = JsonDocument.Parse(responseBody);
        var text = json.RootElement.GetProperty("text").GetString() ?? "";

        _logger.LogInformation("Transcripción exitosa: {Length} caracteres", text.Length);

        return new TranscriptionResult { Exito = true, Texto = text };
    }

    private static string GetMimeType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".mp3" => "audio/mpeg",
            ".mp4" => "audio/mp4",
            ".m4a" => "audio/mp4",
            ".wav" => "audio/wav",
            ".webm" => "audio/webm",
            ".ogg" => "audio/ogg",
            ".oga" => "audio/ogg",
            ".flac" => "audio/flac",
            _ => "application/octet-stream"
        };
    }
}

public class TranscriptionResult
{
    public bool Exito { get; set; }
    public string? Texto { get; set; }
    public string? Error { get; set; }
}
