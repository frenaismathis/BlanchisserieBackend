using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Data;

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

var app = builder.Build();

app.UseHttpsRedirection();
app.Run();
