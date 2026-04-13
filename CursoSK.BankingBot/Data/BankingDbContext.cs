using Microsoft.EntityFrameworkCore;
using CursoSK.BankingBot.Models;

namespace CursoSK.BankingBot.Data;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<DocumentoCliente> DocumentosCliente => Set<DocumentoCliente>();
    public DbSet<SolicitudPrestamo> SolicitudesPrestamo => Set<SolicitudPrestamo>();
    public DbSet<RequisitoSolicitud> RequisitosSolicitud => Set<RequisitoSolicitud>();
    public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();
    public DbSet<LeyNormativa> LeyesNormativas => Set<LeyNormativa>();
    public DbSet<ArticuloLey> ArticulosLey => Set<ArticuloLey>();
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<PluginLog> PluginLogs => Set<PluginLog>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ─── Índices ────────────────────────────────────────
        mb.Entity<Cliente>().HasIndex(c => c.Identidad).IsUnique();
        mb.Entity<SolicitudPrestamo>().HasIndex(s => s.NumeroSolicitud).IsUnique();
        mb.Entity<ChatSession>().HasIndex(s => s.SessionId).IsUnique();
        mb.Entity<LeyNormativa>().HasIndex(l => l.Categoria);

        // ─── Seed Data: Clientes ────────────────────────────
        mb.Entity<Cliente>().HasData(
            new Cliente
            {
                Id = 1,
                Identidad = "0801-1990-12345",
                NombreCompleto = "María Elena Rodríguez López",
                Correo = "maria.rodriguez@email.com",
                Telefono = "+504 9999-1234",
                Direccion = "Col. Kennedy, Tegucigalpa, Honduras",
                FechaNacimiento = new DateTime(1990, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                Ocupacion = "Ingeniera de Sistemas",
                IngresoMensual = 45000m,
                Estado = "activo",
                FechaRegistro = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = 2,
                Identidad = "0501-1985-67890",
                NombreCompleto = "Carlos Alberto Martínez Reyes",
                Correo = "carlos.martinez@empresa.hn",
                Telefono = "+504 9888-5678",
                Direccion = "Res. Los Castaños, San Pedro Sula, Honduras",
                FechaNacimiento = new DateTime(1985, 11, 22, 0, 0, 0, DateTimeKind.Utc),
                Ocupacion = "Contador Público",
                IngresoMensual = 35000m,
                Estado = "activo",
                FechaRegistro = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new Cliente
            {
                Id = 3,
                Identidad = "0101-1995-11223",
                NombreCompleto = "Andrea Fernanda García Mejía",
                Correo = "andrea.garcia@gmail.com",
                Telefono = "+504 3345-7890",
                Direccion = "Bo. El Centro, La Ceiba, Honduras",
                FechaNacimiento = new DateTime(1995, 3, 8, 0, 0, 0, DateTimeKind.Utc),
                Ocupacion = "Comerciante",
                IngresoMensual = 28000m,
                Estado = "activo",
                FechaRegistro = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // ─── Seed Data: Solicitudes de Préstamo ─────────────
        mb.Entity<SolicitudPrestamo>().HasData(
            new SolicitudPrestamo
            {
                Id = 1,
                NumeroSolicitud = "PREST-2026-001",
                ClienteId = 1,
                TipoPrestamo = "personal",
                MontoSolicitado = 150000m,
                PlazoMeses = 36,
                TasaInteres = 18.5m,
                CuotaMensualEstimada = 5475.32m,
                Estado = "pre_aprobado",
                PuntajeRiesgo = 25,
                NivelRiesgo = "bajo",
                FechaSolicitud = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new SolicitudPrestamo
            {
                Id = 2,
                NumeroSolicitud = "PREST-2026-002",
                ClienteId = 2,
                TipoPrestamo = "hipotecario",
                MontoSolicitado = 2500000m,
                PlazoMeses = 240,
                TasaInteres = 9.5m,
                CuotaMensualEstimada = 23280.50m,
                Estado = "documentos_pendientes",
                PuntajeRiesgo = 45,
                NivelRiesgo = "medio",
                FechaSolicitud = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new SolicitudPrestamo
            {
                Id = 3,
                NumeroSolicitud = "PREST-2026-003",
                ClienteId = 3,
                TipoPrestamo = "empresarial",
                MontoSolicitado = 500000m,
                PlazoMeses = 60,
                TasaInteres = 14.0m,
                Estado = "en_revision",
                PuntajeRiesgo = 60,
                NivelRiesgo = "medio",
                FechaSolicitud = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // ─── Seed Data: Requisitos ──────────────────────────
        mb.Entity<RequisitoSolicitud>().HasData(
            // Requisitos solicitud 1 (personal)
            new RequisitoSolicitud { Id = 1, SolicitudPrestamoId = 1, Nombre = "Documento de Identidad (DPI)", Descripcion = "Copia del DPI vigente por ambos lados", Obligatorio = true, Cumplido = true, FechaCumplimiento = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc) },
            new RequisitoSolicitud { Id = 2, SolicitudPrestamoId = 1, Nombre = "Constancia Laboral", Descripcion = "Carta de trabajo emitida en los últimos 30 días", Obligatorio = true, Cumplido = true, FechaCumplimiento = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc) },
            new RequisitoSolicitud { Id = 3, SolicitudPrestamoId = 1, Nombre = "Últimos 3 estados de cuenta", Descripcion = "Estados de cuenta bancarios de los últimos 3 meses", Obligatorio = true, Cumplido = true, FechaCumplimiento = new DateTime(2026, 3, 17, 0, 0, 0, DateTimeKind.Utc) },
            new RequisitoSolicitud { Id = 4, SolicitudPrestamoId = 1, Nombre = "Recibo de servicios públicos", Descripcion = "Recibo reciente de agua, luz o teléfono", Obligatorio = true, Cumplido = false },
            new RequisitoSolicitud { Id = 5, SolicitudPrestamoId = 1, Nombre = "Referencias personales", Descripcion = "Dos referencias personales con teléfono", Obligatorio = false, Cumplido = true, FechaCumplimiento = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc) },

            // Requisitos solicitud 2 (hipotecario)
            new RequisitoSolicitud { Id = 6, SolicitudPrestamoId = 2, Nombre = "Documento de Identidad (DPI)", Obligatorio = true, Cumplido = true, FechaCumplimiento = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc) },
            new RequisitoSolicitud { Id = 7, SolicitudPrestamoId = 2, Nombre = "Avalúo de la propiedad", Descripcion = "Avalúo realizado por perito autorizado", Obligatorio = true, Cumplido = false },
            new RequisitoSolicitud { Id = 8, SolicitudPrestamoId = 2, Nombre = "Escritura de propiedad", Descripcion = "Copia de escritura pública del inmueble", Obligatorio = true, Cumplido = false },
            new RequisitoSolicitud { Id = 9, SolicitudPrestamoId = 2, Nombre = "Constancia de ingresos", Obligatorio = true, Cumplido = true, FechaCumplimiento = new DateTime(2026, 3, 21, 0, 0, 0, DateTimeKind.Utc) },
            new RequisitoSolicitud { Id = 10, SolicitudPrestamoId = 2, Nombre = "Póliza de seguro", Descripcion = "Póliza de seguro contra incendio y terremoto", Obligatorio = true, Cumplido = false },

            // Requisitos solicitud 3 (empresarial)
            new RequisitoSolicitud { Id = 11, SolicitudPrestamoId = 3, Nombre = "RTN de la empresa", Obligatorio = true, Cumplido = true, FechaCumplimiento = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RequisitoSolicitud { Id = 12, SolicitudPrestamoId = 3, Nombre = "Estados financieros auditados", Descripcion = "Últimos 2 años de estados financieros auditados", Obligatorio = true, Cumplido = false },
            new RequisitoSolicitud { Id = 13, SolicitudPrestamoId = 3, Nombre = "Plan de negocios", Descripcion = "Plan de inversión y retorno", Obligatorio = true, Cumplido = false }
        );

        // ─── Seed Data: Historial de estados ────────────────
        mb.Entity<HistorialEstado>().HasData(
            new HistorialEstado { Id = 1, SolicitudPrestamoId = 1, EstadoAnterior = "borrador", EstadoNuevo = "en_revision", Observacion = "Solicitud enviada por el cliente", Usuario = "sistema", Fecha = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc) },
            new HistorialEstado { Id = 2, SolicitudPrestamoId = 1, EstadoAnterior = "en_revision", EstadoNuevo = "pre_aprobado", Observacion = "Análisis crediticio favorable. Puntaje: 25 (bajo riesgo)", Usuario = "analista_credito", Fecha = new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc) },
            new HistorialEstado { Id = 3, SolicitudPrestamoId = 2, EstadoAnterior = "borrador", EstadoNuevo = "en_revision", Usuario = "sistema", Fecha = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc) },
            new HistorialEstado { Id = 4, SolicitudPrestamoId = 2, EstadoAnterior = "en_revision", EstadoNuevo = "documentos_pendientes", Observacion = "Faltan avalúo, escritura y póliza de seguro", Usuario = "analista_credito", Fecha = new DateTime(2026, 3, 22, 0, 0, 0, DateTimeKind.Utc) }
        );

        // ─── Seed Data: Leyes y Normativas ──────────────────
        mb.Entity<LeyNormativa>().HasData(
            new LeyNormativa
            {
                Id = 1,
                Titulo = "Ley del Sistema Financiero",
                NumeroLey = "Decreto 129-2004",
                Categoria = "bancaria",
                Contenido = @"TÍTULO I - DISPOSICIONES GENERALES
Art. 1. La presente Ley tiene por objeto regular la organización, constitución, funcionamiento, fusión, conversión y liquidación de las instituciones del sistema financiero.
Art. 2. El sistema financiero está constituido por las instituciones bancarias, sociedades financieras, asociaciones de ahorro y préstamo, y las demás instituciones autorizadas por la CNBS.
Art. 15. Las instituciones del sistema financiero deberán mantener un capital mínimo y reservas de acuerdo a las regulaciones emitidas por la CNBS.
Art. 20. Toda persona natural o jurídica que desee obtener un préstamo mayor a L.500,000 deberá presentar estados financieros auditados.
Art. 45. Las instituciones deberán implementar políticas de conocimiento del cliente (KYC) y prevención de lavado de activos.",
                FechaPublicacion = new DateTime(2004, 9, 20, 0, 0, 0, DateTimeKind.Utc),
                Vigente = true
            },
            new LeyNormativa
            {
                Id = 2,
                Titulo = "Ley Contra el Lavado de Activos",
                NumeroLey = "Decreto 144-2014",
                Categoria = "lavado_activos",
                Contenido = @"Art. 1. La presente ley tiene como objetivo prevenir, detectar y sancionar el lavado de activos y el financiamiento del terrorismo.
Art. 3. Son sujetos obligados: bancos, sociedades financieras, casas de cambio, cooperativas de ahorro y crédito, aseguradoras y demás instituciones financieras.
Art. 8. Los sujetos obligados deberán implementar un programa de cumplimiento que incluya: identificación del cliente, monitoreo de transacciones, reporte de operaciones sospechosas.
Art. 12. Toda transacción en efectivo igual o superior a US$10,000 o su equivalente deberá ser reportada a la UIF.
Art. 15. Los sujetos obligados deberán conservar los registros de transacciones por un período mínimo de 5 años.
Art. 20. Las sanciones por incumplimiento van desde multas hasta la cancelación de la licencia de operación.",
                FechaPublicacion = new DateTime(2014, 12, 30, 0, 0, 0, DateTimeKind.Utc),
                Vigente = true
            },
            new LeyNormativa
            {
                Id = 3,
                Titulo = "Ley de Protección al Consumidor Financiero",
                NumeroLey = "Decreto 170-2016",
                Categoria = "proteccion_consumidor",
                Contenido = @"Art. 1. Objeto: Proteger los derechos de los consumidores de productos y servicios financieros.
Art. 5. Los consumidores financieros tienen derecho a: información clara y oportuna, trato justo, protección de datos personales, acceso a mecanismos de reclamo.
Art. 10. Las instituciones financieras deben informar al consumidor sobre: tasa de interés efectiva, comisiones, seguros, penalidades por mora, y el costo total del crédito.
Art. 15. Se prohíbe: cobrar comisiones no pactadas, modificar unilateralmente las condiciones del contrato, realizar prácticas abusivas de cobranza.
Art. 22. Los contratos de adhesión deben ser aprobados por la CNBS y redactados en lenguaje claro y comprensible.
Art. 30. El consumidor tiene derecho a realizar pagos anticipados sin penalidad cuando el contrato sea menor a 5 años.",
                FechaPublicacion = new DateTime(2016, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                Vigente = true
            },
            new LeyNormativa
            {
                Id = 4,
                Titulo = "Normas de Gestión de Riesgo Crediticio",
                NumeroLey = "Resolución CNBS-GES-041-2019",
                Categoria = "bancaria",
                Contenido = @"Art. 1. Estas normas establecen los criterios para la evaluación y clasificación de la cartera crediticia.
Art. 5. Categorías de clasificación: A (Normal), B (Potencial), C (Deficiente), D (Dudosa), E (Irrecuperable).
Art. 8. Criterios de evaluación: capacidad de pago, historial crediticio, garantías, situación del sector económico.
Art. 12. Relación cuota/ingreso: No deberá exceder el 40% de los ingresos netos mensuales del deudor para créditos de consumo.
Art. 15. Para créditos hipotecarios, la relación cuota/ingreso no deberá exceder el 30%.
Art. 20. Las instituciones deberán constituir provisiones según la categoría de riesgo: A=1%, B=5%, C=20%, D=50%, E=100%.",
                FechaPublicacion = new DateTime(2019, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                Vigente = true
            }
        );

        // ─── Seed Data: Artículos de Ley ────────────────────
        mb.Entity<ArticuloLey>().HasData(
            new ArticuloLey { Id = 1, LeyNormativaId = 4, NumeroArticulo = "Art. 12", Texto = "Relación cuota/ingreso: No deberá exceder el 40% de los ingresos netos mensuales del deudor para créditos de consumo.", Resumen = "Límite cuota/ingreso 40% para consumo" },
            new ArticuloLey { Id = 2, LeyNormativaId = 4, NumeroArticulo = "Art. 15", Texto = "Para créditos hipotecarios, la relación cuota/ingreso no deberá exceder el 30%.", Resumen = "Límite cuota/ingreso 30% para hipotecarios" },
            new ArticuloLey { Id = 3, LeyNormativaId = 3, NumeroArticulo = "Art. 10", Texto = "Las instituciones financieras deben informar al consumidor sobre: tasa de interés efectiva, comisiones, seguros, penalidades por mora, y el costo total del crédito.", Resumen = "Obligación de información completa al consumidor" },
            new ArticuloLey { Id = 4, LeyNormativaId = 2, NumeroArticulo = "Art. 12", Texto = "Toda transacción en efectivo igual o superior a US$10,000 o su equivalente deberá ser reportada a la UIF.", Resumen = "Reporte obligatorio transacciones >= $10,000" }
        );

        // ─── Seed Data: Chat Sessions ───────────────────────
        mb.Entity<ChatSession>().HasData(
            new ChatSession
            {
                Id = 1,
                SessionId = "onboarding-maria-001",
                Titulo = "Onboarding - Solicitud préstamo personal",
                UsuarioId = "maria.rodriguez",
                Canal = "web",
                FechaCreacion = new DateTime(2026, 4, 5, 9, 0, 0, DateTimeKind.Utc),
                UltimaActividad = new DateTime(2026, 4, 5, 9, 30, 0, DateTimeKind.Utc),
                Activa = true
            },
            new ChatSession
            {
                Id = 2,
                SessionId = "legal-consulta-001",
                Titulo = "Consulta legal - Tasas de interés",
                UsuarioId = "analista.legal",
                Canal = "web",
                FechaCreacion = new DateTime(2026, 4, 6, 14, 0, 0, DateTimeKind.Utc),
                UltimaActividad = new DateTime(2026, 4, 6, 14, 15, 0, DateTimeKind.Utc),
                Activa = true
            }
        );

        mb.Entity<ChatMessage>().HasData(
            // Sesión 1: Onboarding
            new ChatMessage { Id = 1, ChatSessionId = 1, Rol = "system", Contenido = "Eres un asistente bancario especializado en onboarding de clientes y solicitudes de préstamos. Guías al cliente paso a paso, verificas requisitos y explicas el proceso de forma clara.", Fecha = new DateTime(2026, 4, 5, 9, 0, 0, DateTimeKind.Utc) },
            new ChatMessage { Id = 2, ChatSessionId = 1, Rol = "user", Contenido = "Hola, quiero solicitar un préstamo personal de L.150,000 a 36 meses.", Fecha = new DateTime(2026, 4, 5, 9, 1, 0, DateTimeKind.Utc) },
            new ChatMessage { Id = 3, ChatSessionId = 1, Rol = "assistant", Contenido = "¡Bienvenida! Con gusto le ayudo con su solicitud de préstamo personal.\n\nPara un préstamo de L.150,000 a 36 meses, la tasa de interés actual es del 18.5% anual, lo que genera una cuota mensual estimada de L.5,475.32.\n\nNecesitaremos los siguientes documentos:\n1. ✅ Documento de Identidad (DPI) vigente\n2. ✅ Constancia laboral reciente (últimos 30 días)\n3. ✅ Últimos 3 estados de cuenta bancarios\n4. 📋 Recibo de servicios públicos\n5. 📋 Referencias personales (2)\n\n¿Desea proceder con la solicitud?", Fecha = new DateTime(2026, 4, 5, 9, 1, 30, DateTimeKind.Utc) },
            new ChatMessage { Id = 4, ChatSessionId = 1, Rol = "user", Contenido = "Sí, ya tengo mi DPI y constancia laboral listos. Mi ingreso mensual es de L.45,000.", Fecha = new DateTime(2026, 4, 5, 9, 3, 0, DateTimeKind.Utc) },
            new ChatMessage { Id = 5, ChatSessionId = 1, Rol = "assistant", Contenido = "Excelente. Con un ingreso de L.45,000 y una cuota de L.5,475.32, su relación cuota/ingreso sería del 12.2%, lo cual está dentro del límite permitido del 40% (Art. 12, Normas de Gestión de Riesgo Crediticio).\n\n✅ Pre-calificación: APROBADA\n\nSiguiente paso: Por favor adjunte sus documentos escaneados.", Fecha = new DateTime(2026, 4, 5, 9, 3, 30, DateTimeKind.Utc) },

            // Sesión 2: Consulta legal
            new ChatMessage { Id = 6, ChatSessionId = 2, Rol = "system", Contenido = "Eres un asistente legal bancario experto en regulación financiera hondureña. Respondes consultas citando las leyes y normativas aplicables.", Fecha = new DateTime(2026, 4, 6, 14, 0, 0, DateTimeKind.Utc) },
            new ChatMessage { Id = 7, ChatSessionId = 2, Rol = "user", Contenido = "¿Cuál es el límite de relación cuota/ingreso para un crédito hipotecario?", Fecha = new DateTime(2026, 4, 6, 14, 1, 0, DateTimeKind.Utc) },
            new ChatMessage { Id = 8, ChatSessionId = 2, Rol = "assistant", Contenido = "Según el Art. 15 de las Normas de Gestión de Riesgo Crediticio (Resolución CNBS-GES-041-2019):\n\n\"Para créditos hipotecarios, la relación cuota/ingreso no deberá exceder el 30%.\"\n\nEsto significa que la cuota mensual del crédito hipotecario no debe superar el 30% de los ingresos netos mensuales verificables del deudor.\n\nAdicional, el Art. 12 de la misma norma establece que para créditos de consumo, el límite es del 40%.", Fecha = new DateTime(2026, 4, 6, 14, 1, 30, DateTimeKind.Utc) }
        );
    }
}
