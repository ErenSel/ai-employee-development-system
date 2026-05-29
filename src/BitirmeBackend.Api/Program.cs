using System.Text;
using BitirmeBackend.Api.Middleware;
using BitirmeBackend.Application.Interfaces;
using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Application.Services;
using BitirmeBackend.Infrastructure.ExternalServices;
using BitirmeBackend.Infrastructure.MockRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

// Configure Serilog before builder
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .Build())
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// ── Swagger / OpenAPI ──────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bitirme Backend API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT token giriniz. Örnek: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ── JWT Authentication ─────────────────────────────────────────────────────────
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is missing");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",    p => p.RequireRole("Admin"));
    options.AddPolicy("HrOrManager", p => p.RequireRole("HR", "Manager"));
    options.AddPolicy("EmployeeOnly", p => p.RequireRole("Employee"));
    options.AddPolicy("Authenticated", p => p.RequireAuthenticatedUser());
});

// ── CORS ───────────────────────────────────────────────────────────────────────
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? builder.Configuration["AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries)
    ?? ["http://localhost:3000"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ── Controllers ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Mock Repositories (DI) ─────────────────────────────────────────────────────
builder.Services.AddSingleton<IUserRepository, MockUserRepository>();
builder.Services.AddSingleton<IRoleRepository, MockRoleRepository>();
builder.Services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
builder.Services.AddSingleton<IAssessmentRepository, MockAssessmentRepository>();
builder.Services.AddSingleton<IActionCatalogRepository, MockActionCatalogRepository>();
builder.Services.AddSingleton<IModelVersionRepository, MockModelVersionRepository>();
builder.Services.AddSingleton<IAiPredictionRepository, MockAiPredictionRepository>();
builder.Services.AddSingleton<IActionPlanRepository, MockActionPlanRepository>();
builder.Services.AddSingleton<IEmployeeTaskRepository, MockEmployeeTaskRepository>();
builder.Services.AddSingleton<IRefreshTokenRepository, MockRefreshTokenRepository>();
builder.Services.AddSingleton<IUnitOfWork, MockUnitOfWork>();

// FakeMlPredictionClient must NOT be used in Production
if (!builder.Environment.IsProduction())
    builder.Services.AddSingleton<IMlPredictionClient, FakeMlPredictionClient>();

builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// ── Build ──────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Middleware pipeline ────────────────────────────────────────────────────────
// Order: ExceptionHandling → CorrelationId → Serilog → CORS → Auth → Controllers
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bitirme Backend API v1"));

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
