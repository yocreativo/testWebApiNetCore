using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasAutosController : ControllerBase
    {
        private readonly pruebaContext _context;

        public MarcasAutosController(pruebaContext context)
        {
            _context = context;
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
    }
}
