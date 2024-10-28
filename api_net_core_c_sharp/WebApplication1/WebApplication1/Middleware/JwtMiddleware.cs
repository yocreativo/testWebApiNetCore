namespace WebApplication1.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            // Permitir acceso a la ruta de login
            if (context.Request.Path == "/api/MarcasAutos/login" || context.Request.Path == "/api/MarcasAutos")
            {
                await _next(context);
                return;
            }

            // Obtener el token del encabezado de autorización
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Authorization token is required.");
                return;
            }

            try
            {
                // Validar el token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["SecretKey"]); // Asegúrate de tener esta clave en tu appsettings.json
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                // Obtener los datos del usuario
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;
                context.Items["User"] = userEmail; // Almacena el usuario en el contexto

                // Aquí se debe agregar lógica para verificar si el usuario existe en la base de datos

                //////////////////////////////////////////////////////////////////////////////////////
                //permitir que la solicitud continúe su flujo a través de la cadena de middleware
                //lo que permite que la solicitud continúe siendo procesada
                await _next(context);
            }
            catch
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid token.");
            }
        }
    }

}
