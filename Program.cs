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
    config.OperationProcessors.Add(new OperationSecurityScopeProcessor("apiKey"));
    config.DocumentProcessors.Add(new SecurityDefinitionAppender("apiKey", new NSwag.OpenApiSecurityScheme()
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Bearer token"
    }));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
