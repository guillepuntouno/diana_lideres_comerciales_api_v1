using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DIANA.Maestros.Data;
using DIANA.Maestros.Models.Entities;
 

namespace DIANA.Maestros.Repositories
{
    public class LiderComercialRepository : ILiderComercialRepository
    {
        private readonly DianaDbContext _context;

        public LiderComercialRepository(DianaDbContext context)
        {
            _context = context;
        }

        public async Task<LiderComercialEntity> ObtenerPorClaveAsync(string clave)
        {
            var lider = await _context.LideresComerciales
                .Include(l => l.Rutas)
                .FirstOrDefaultAsync(l => l.Clave == clave);

            if (lider != null && lider.Rutas != null)
            {
                foreach (var ruta in lider.Rutas)
                {
                    await _context.Entry(ruta)
                        .Collection(r => r.Negocios)
                        .LoadAsync();
                }
            }

            return lider;
        }
    }
}