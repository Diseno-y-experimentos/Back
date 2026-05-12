using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using BusTrackBackEnd.API.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using BusTrackBackEnd.API.IAM.Infrastructure.Tokens.JWT.Configuration;
// --- IAM USINGS ---
using BusTrackBackEnd.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using BusTrackBackEnd.API.IAM.Application.Internal.OutboundServices;
using BusTrackBackEnd.API.IAM.Domain.Repositories;
using BusTrackBackEnd.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using BusTrackBackEnd.API.IAM.Domain.Services;
using BusTrackBackEnd.API.IAM.Application.Internal.CommandServices;
using BusTrackBackEnd.API.IAM.Application.Internal.QueryServices;
using BusTrackBackEnd.API.IAM.Infrastructure.Tokens.JWT.Services;
using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates; 

// --- ROUTES USINGS ---
using BusTrackBackEnd.API.Routes.Domain.Repositories;
using BusTrackBackEnd.API.Routes.Infrastructure.Persistence.EFC.Repositories;
using BusTrackBackEnd.API.Routes.Domain.Services;
using BusTrackBackEnd.API.Routes.Application.Internal.CommandServices;
using BusTrackBackEnd.API.Routes.Application.Internal.QueryServices;

// --- COMPANIES USINGS ---
using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates; 
using BusTrackBackEnd.API.Companies.Infrastructure.Persistence.EFC.Repositories;
using BusTrackBackEnd.API.Companies.Domain.Services;
using BusTrackBackEnd.API.Companies.Application.CommandServices;
using BusTrackBackEnd.API.Companies.Application.QueryServices;

// --- TRANSPORT USINGS ---
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Repositories;
using BusTrackBackEnd.API.BoundedContexts.Transport.Infrastructure.Persistence;
using BusTrackBackEnd.API.BoundedContexts.Transport.Application.Internal.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// 1. CONFIGURACIÓN DE CORS (IMPORTANTE PARA FRONTEND)
// ====================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// ====================================================================
// 2. CONFIGURACIÓN DE CONTROLADORES Y RUTAS
// ====================================================================
builder.Services.AddControllers(options => 
{
    options.Conventions.Add(new KebabCaseRouteNamingConvention());
});

// ====================================================================
// 3. CONFIGURACIÓN DE BASE DE DATOS (MYSQL LOCAL)
// ====================================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (connectionString != null)
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .LogTo(s => Console.WriteLine(s), LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
});

// ====================================================================
// 4. OPENAPI / SWAGGER
// ====================================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BusTrack API",
        Version = "v1",
        Description = "API para gestión de transporte urbano - BusTrack"
    });
});

// ====================================================================
// 5. INYECCIÓN DE DEPENDENCIAS
// ====================================================================

// --- Shared Bounded Context ---
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// --- IAM Bounded Context ---
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

// --- Routes Bounded Context ---
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteCommandService, RouteCommandService>();
builder.Services.AddScoped<IRouteQueryService, RouteQueryService>();

// --- Companies Bounded Context ---
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyCommandService, CompanyCommandService>();
builder.Services.AddScoped<ICompanyQueryService, CompanyQueryService>();

// --- Transport Bounded Context ---
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IBusService, BusService>();


var app = builder.Build();

// ====================================================================
// 6. PIPELINE DE LA APLICACIÓN
// ====================================================================

// --- CONFIGURACIÓN DE SWAGGER ---
// Quitamos el 'if (IsDevelopment)' para que funcione en Producción
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    // Esto hace que Swagger aparezca en la ruta raíz (midominio.com/)
    // en lugar de midominio.com/swagger
    options.RoutePrefix = string.Empty; 
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Activamos CORS antes de la autorización
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();