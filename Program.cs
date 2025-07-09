using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Data;
using BlanchisserieBackend.Services;

// Chargement des variables d'environnement
Env.Load();
var builder = WebApplication.CreateBuilder(args);

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
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
