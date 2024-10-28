using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Service
{
    public class ServiceSP
    {
        private readonly pruebaContext _context;

        public ServiceSP(pruebaContext context)
        {
            _context = context;
        }

        public async Task<List<SPMarcasAutos>> EjecutarProcedimientoAlmacenado(string parametro)
        {
            var resultado = await _context.Set<SPMarcasAutos>()
                .FromSqlRaw("EXEC NombreDelProcedimiento @parametro = {0}", parametro)
                .ToListAsync();

            return resultado;
        }
    }
}
