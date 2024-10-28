using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using WebApplication1.Middleware;
using WebApplication1.Service;

var AllowAllOrigins = "";

var builder = WebApplication.CreateBuilder(args);

//tokens
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
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "tu_dominio_o_nombre_de_emisor",
            ValidAudience = "tu_dominio_o_nombre_de_audiencia",
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("tu_clave_secreta"))
        };
    });

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

builder.Services.AddScoped<ServiceSP>();

var app = builder.Build();

//En el modo de desarrollo se usa el Swagger
//para probar los endpoints

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AllowAllOrigins);

// Agregar el middleware de manejo de errores
//app.UseMiddleware<ErrorHandlingMiddleware>();

//app.UseMiddleware<JwtMiddleware>();

//app.UseForwardedHeaders(new ForwardedHeadersOptions
//{
//    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
//});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
