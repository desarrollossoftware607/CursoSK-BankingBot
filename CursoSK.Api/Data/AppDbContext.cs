using Microsoft.EntityFrameworkCore;
using CursoSK.Api.Models;

namespace CursoSK.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<PluginInvocationLog> PluginLogs => Set<PluginInvocationLog>();
    public DbSet<ContenidoGenerado> ContenidosGenerados => Set<ContenidoGenerado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatSession>()
            .HasIndex(s => s.SessionId).IsUnique();

        modelBuilder.Entity<ChatSession>().HasData(
            new ChatSession { Id = 1, SessionId = "demo-session", Titulo = "Sesión de demostración",
                UsuarioId = "instructor", FechaCreacion = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UltimaActividad = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<ChatMessage>().HasData(
            new ChatMessage { Id = 1, ChatSessionId = 1, Rol = "system",
                Contenido = "Eres un asistente del curso de Semantic Kernel.",
                Fecha = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new ChatMessage { Id = 2, ChatSessionId = 1, Rol = "user",
                Contenido = "¿Qué es Semantic Kernel?",
                Fecha = new DateTime(2026, 1, 1, 0, 0, 1, DateTimeKind.Utc) },
            new ChatMessage { Id = 3, ChatSessionId = 1, Rol = "assistant",
                Contenido = "Semantic Kernel es un SDK open-source de Microsoft que permite integrar LLMs en aplicaciones C#, Python y Java.",
                Fecha = new DateTime(2026, 1, 1, 0, 0, 2, DateTimeKind.Utc) }
        );
    }
}
