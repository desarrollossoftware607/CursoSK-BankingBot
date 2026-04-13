using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using CursoSK.BankingBot.Data;
using CursoSK.BankingBot.Plugins;
using CursoSK.BankingBot.Filters;
using CursoSK.BankingBot.Services;

var builder = WebApplication.CreateBuilder(args);

// ─── ASP.NET Core ────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Banking Bot - Semantic Kernel API", Version = "v1" });
});

// ─── Entity Framework + SQLite ───────────────────────────────
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=banking.db"));

// ─── HttpClient ──────────────────────────────────────────────
builder.Services.AddHttpClient();

// ─── Semantic Kernel ─────────────────────────────────────────
#pragma warning disable SKEXP0010, SKEXP0070
var kernelBuilder = Kernel.CreateBuilder();

var provider = builder.Configuration["LLM:Provider"]?.ToLower() ?? "azure";

if (provider == "openai")
{
    var apiKey = builder.Configuration["LLM:OpenAI:ApiKey"]!;
    kernelBuilder.AddOpenAIChatCompletion(
        modelId: builder.Configuration["LLM:OpenAI:Model"] ?? "gpt-4o-mini",
        apiKey: apiKey);
    kernelBuilder.AddOpenAITextEmbeddingGeneration(
        modelId: "text-embedding-ada-002",
        apiKey: apiKey);
}
else // azure
{
    var endpoint = builder.Configuration["LLM:Azure:Endpoint"]!;
    var apiKey = builder.Configuration["LLM:Azure:ApiKey"]!;
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: builder.Configuration["LLM:Azure:DeploymentName"] ?? "gpt-4o-mini",
        endpoint: endpoint,
        apiKey: apiKey);

    // ─── Embedding (para RAG / Vector Store) ─────────────
    var embeddingEndpoint = builder.Configuration["LLM:Embedding:Endpoint"] ?? endpoint;
    var embeddingKey = builder.Configuration["LLM:Embedding:ApiKey"] ?? apiKey;
    var embeddingModel = builder.Configuration["LLM:Embedding:DeploymentName"] ?? "text-embedding-ada-002";
    kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(embeddingModel, embeddingEndpoint, embeddingKey);
}

// Filtro de auditoría
kernelBuilder.Services.AddLogging();
kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter>(sp =>
    new AuditFilter(sp.GetRequiredService<ILogger<AuditFilter>>()));
#pragma warning restore SKEXP0010, SKEXP0070

var kernel = kernelBuilder.Build();

// ─── Registrar Plugins ───────────────────────────────────────
kernel.Plugins.AddFromObject(
    new OnboardingPlugin(builder.Services.BuildServiceProvider()),
    "OnboardingPlugin");
kernel.Plugins.AddFromObject(
    new LegalPlugin(builder.Services.BuildServiceProvider()),
    "LegalPlugin");
kernel.Plugins.AddFromObject(
    new CalculadoraFinancieraPlugin(),
    "CalculadoraPlugin");

builder.Services.AddSingleton(kernel);

// ─── Servicios ───────────────────────────────────────────────
builder.Services.AddScoped<ConversationService>();
builder.Services.AddSingleton<VectorStoreService>();
builder.Services.AddScoped<LegalIndexingService>();
builder.Services.AddSingleton<AudioTranscriptionService>();

var app = builder.Build();

// ─── Asegurar BD y seed data ─────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
    db.Database.EnsureCreated();
}

// ─── Pipeline HTTP ───────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Bot API v1");
    c.DocumentTitle = "Banking Bot - Swagger";
    c.RoutePrefix = "swagger";
});

app.MapControllers();

// Redirigir raíz a Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
