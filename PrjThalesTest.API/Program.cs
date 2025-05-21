using Microsoft.OpenApi.Models;
using PrjThalesTest.Business;
using PrjThalesTest.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Productos API",
        Version = "v1",
        Description = "API para gestión de productos con cálculo de impuestos"
    });
});

// Registrar servicios de capas
builder.Services.AddDataAccess();
builder.Services.AddBusiness();

// Configuración CORS para permitir solicitudes desde la aplicación MVC
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvcApp",
        policy => policy
            .WithOrigins("https://localhost:5001") // URL de la aplicación MVC
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowMvcApp");
app.UseAuthorization();
app.MapControllers();

app.Run();