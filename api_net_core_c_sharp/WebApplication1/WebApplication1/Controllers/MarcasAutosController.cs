using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasAutosController : ControllerBase
    {
        private readonly pruebaContext _context;

        private ServiceSP _miServicio;

        public MarcasAutosController(pruebaContext context)
        {
            _context = context;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MarcasAutosController(ServiceSP miServicio)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _miServicio = miServicio;
        }
        // GET: api/MarcasAutos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcasAutos>>> GetMarcasAutos()
        {
          if (_context.marcasAutos == null)
          {
              return NotFound();
          }
            return await _context.marcasAutos.ToListAsync();
        }
        // GET: api/MarcaAuto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MarcasAutos>> GetMarcaAuto(int id)
        {
            if (_context.marcasAutos == null)
            {
                return NotFound();
            }

            var marcaAuto = await _context.marcasAutos.FindAsync(id);

            if (marcaAuto == null)
            {
                return NotFound();
            }

            return marcaAuto;
        }

        private const string SecretKey = "12345678901234567890123456789012"; // clave segura

        [HttpPost("login")]
        //POST: api/Login/
        public IActionResult Login([FromBody] LoginModel model)
        {

            // Validar usuario (esto es solo un ejemplo)
            if (model.Username == "reynol" && model.Password == "trd.2020")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, model.Username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { Token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized();
        }

        [HttpGet("SP/{parametro}")]
        public async Task<IActionResult> Get(string parametro)
        {

            /// Utilizando un servicio/////////////////////////////////////////////////
            var resultado = await _miServicio.EjecutarProcedimientoAlmacenado(parametro);
            //return Ok(resultado);
            /////////////////////////////////////////////////////////////////////////////
            //Utilizando directamente en el controlador
            ////////////////////////////////////////////////////////////////////////////
            List<SPMarcasAutos>? resultado1 = await _context.Set<SPMarcasAutos>()
            .FromSqlRaw("EXEC NombreDelProcedimiento @parametro = {0}", parametro)
            .ToListAsync();

            return Ok(resultado1);
            ///////////////////////////////////////////////////////////////////////////
        }
    }
}
