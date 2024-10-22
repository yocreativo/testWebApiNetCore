using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var AllowAllOrigins = "";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<pruebaContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//CONFIGURACION DE CORS
//Aqui se pueden configurar 
//Las url clientes que pueden acceder
//Asi como los m�todos y los encabezados permitidos
/// <summary>
/// ///////////////////////////////////////////////
/// //En este caso no se han configurado restricciones, todo esta permitido
/// </summary>
builder.Services.AddCors(options => {
    options.AddPolicy(AllowAllOrigins, builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

//En el modo de desarrollo se usa el Swagger
//para probar los endpoints

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AllowAllOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
