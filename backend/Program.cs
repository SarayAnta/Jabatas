using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using backend.Models;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Cargar las variables de entorno desde el archivo .env
DotNetEnv.Env.Load();

// Leer la variable de entorno para la cadena de conexión
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");

if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new InvalidOperationException("MongoDB connection string is not set.");
}

// Configurar los servicios de MongoDB
builder.Services.Configure<MongoDBSettings>(options =>
{
    options.ConnectionString = mongoConnectionString;
    options.DatabaseName = builder.Configuration.GetSection("MongoDBSettings:DatabaseName").Value;
});

// Agregar el servicio de MongoDB
builder.Services.AddSingleton<IMongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

// Otros servicios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Aquí puedes agregar tu servicio de acceso a datos si aún no lo has hecho
builder.Services.AddSingleton<EquipoService>();

var app = builder.Build();

// Configurar la tubería de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
