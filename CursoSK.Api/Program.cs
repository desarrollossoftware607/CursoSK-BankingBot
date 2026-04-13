using Microsoft.SemanticKernel;
using Microsoft.EntityFrameworkCore;
using CursoSK.Api.Data;
using CursoSK.Api.Plugins;
using CursoSK.Api.Services;
using CursoSK.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CursoSK.Api", Version = "v1",
        Description = "API del curso Máster en Semantic Kernel — crece sesión a sesión" });
});

// --- EF Core + SQLite ---
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Semantic Kernel ---
var llmProvider = builder.Configuration["LLMSettings:Provider"]?.ToLower() ?? "azure";
var kernelBuilder = Kernel.CreateBuilder();

if (llmProvider == "openai")
{
    kernelBuilder.AddOpenAIChatCompletion(
        modelId: builder.Configuration["LLMSettings:OpenAI:ModelId"] ?? "gpt-4o-mini",
        apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!);
}
else
{
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: builder.Configuration["LLMSettings:AzureOpenAI:DeploymentName"]!,
        endpoint: builder.Configuration["LLMSettings:AzureOpenAI:Endpoint"]!,
        apiKey: builder.Configuration["LLMSettings:AzureOpenAI:ApiKey"]!);
}

// Plugins
var kernel = kernelBuilder.Build();
kernel.Plugins.AddFromObject(new ClimaPlugin(), "Clima");
kernel.Plugins.AddFromObject(new MathPlugin(), "Matematica");
kernel.FunctionInvocationFilters.Add(new LoggingFilter(
    builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<LoggingFilter>()));

builder.Services.AddSingleton(kernel);

// --- Servicios ---
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<ChatSessionService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
