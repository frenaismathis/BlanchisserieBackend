using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Data;
using BlanchisserieBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.Generation.Processors.Security;

// Chargement des variables d'environnement
Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Configuration JWT
var jwtKey = Env.GetString("JWT_SECRET");
var jwtIssuer = Env.GetString("JWT_ISSUER");
builder.Services.AddSingleton<IJwtTokenService>(new JwtTokenService(jwtKey, jwtIssuer));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
    // Ajout pour lire le token depuis le cookie "access_token"
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("access_token"))
            {
                context.Token = context.Request.Cookies["access_token"];
            }
            return Task.CompletedTask;
        }
    };
});

var dbHost = Env.GetString("DB_HOST");
var dbPort = Env.GetString("DB_PORT");
var dbName = Env.GetString("DB_NAME");
var dbUser = Env.GetString("DB_USER");
var dbPassword = Env.GetString("DB_PASSWORD");

// Chaine de caractère pour la connexion à PostgreSQL
var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";


// Ajout du context de PostgreSQL pour Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Injection des services métier
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<ClientOrderArticleService>();
builder.Services.AddScoped<ClientOrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();

builder.Services.AddControllers();

// Configuration pour Swagger via NSwag
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Blanchisserie API";
    config.Description = "⚠️ L'authentification réelle utilise un cookie HTTP-only nommé 'access_token'.\n" +
                         "Swagger utilise le header Authorization uniquement pour les tests locaux. " +
                         "Pour tester l'authentification via cookie, utilisez un client HTTP (Postman, navigateur, etc.).";
    config.OperationProcessors.Add(new OperationSecurityScopeProcessor("apiKey"));
    config.DocumentProcessors.Add(new SecurityDefinitionAppender("apiKey", new NSwag.OpenApiSecurityScheme()
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Bearer token"
    }));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
