using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SmtpD.Core.Repositories;
using SmtpD.Core.Services.Domain;
using SmtpD.Core.Services.Domain.Impl;
using SmtpD.Infrastructure.Sqlite.Contexts;
using SmtpD.Infrastructure.Sqlite.Repositories;
using SmtpD.Web.Hubs;
using SmtpD.Web.Smtp;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSpaStaticFiles(options => options.RootPath = "..\\lib/dist");

builder.Services.AddTransient<IConfigRepository, ConfigRepository>();
builder.Services.AddTransient<IEmailRepository, EmailRepository>();

builder.Services.AddTransient<IConfigService, ConfigService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddHostedService<SmtpServer>();

string path = Directory.GetCurrentDirectory();

string dataDirectory = builder.Environment.IsDevelopment() ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SmtpD", "data") : Path.Combine(path, "..\\data");

if (!Directory.Exists(dataDirectory))
    Directory.CreateDirectory(dataDirectory);

string dbPath = Path.Combine(dataDirectory, "data.db");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));

string logDirectory = builder.Environment.IsDevelopment() ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SmtpD", "logs") : Path.Combine(path, "..\\logs");

if (!Directory.Exists(logDirectory))
    Directory.CreateDirectory(logDirectory);

string logFile = Path.Combine(logDirectory, "logs.log");

builder.Host.UseSerilog((webHostBuilderContext, loggerConfiguration) => loggerConfiguration
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Fatal)
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
    .WriteTo.File(logFile, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5)
);

builder.Services.AddSignalR();

builder.Services.AddCors();

var app = builder.Build();

using var scope = app.Services.CreateScope();

ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Avvio dell'applicazione..");

ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

if (dbContext.Database.GetPendingMigrations().Any())
    dbContext.Database.Migrate();

logger.LogInformation("Verifiche DB completate!");

IConfigService configService = scope.ServiceProvider.GetRequiredService<IConfigService>();
await configService.InitializeAsync();

logger.LogInformation("Verifiche configurazione completate!");

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true) // allow any origin
        .AllowCredentials());
}

if (app.Environment.IsProduction())
    app.UseSpaStaticFiles();

app.MapControllers();

app.MapHub<EmailHub>("/hub/email");

if (app.Environment.IsProduction())
    app.UseSpa(options => options.Options.SourcePath = "..\\lib");

Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => {
    logger.LogInformation("Arresto applicazione in corso..");
};

app.Run("http://localhost:55555");
