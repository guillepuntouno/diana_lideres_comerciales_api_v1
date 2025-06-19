using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DIANA.Maestros.Data;
using DIANA.Maestros.Models.Entities;

namespace DIANA.Maestros.Repositories
{
    public class PlanTrabajoRepository : IPlanTrabajoRepository
    {
        private readonly DianaDbContext _context;

        public PlanTrabajoRepository(DianaDbContext context)
        {
            _context = context;
        }

        public async Task<PlanTrabajoEntity> CrearPlanAsync(PlanTrabajoEntity plan)
        {
            _context.PlanesTrabajos.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<PlanTrabajoEntity> ObtenerPlanPorClaveYSemanaAsync(string liderClave, int semana)
        {
            var plan = await _context.PlanesTrabajos
                .Include(p => p.Dias)
                .FirstOrDefaultAsync(p => p.LiderClave == liderClave && p.Semana == semana);

            // Cargar clientes asignados para cada día por separado
            if (plan?.Dias != null)
            {
                foreach (var dia in plan.Dias)
                {
                    await _context.Entry(dia)
                        .Collection(d => d.ClientesAsignados)
                        .LoadAsync();
                }
            }

            return plan;
        }

        public async Task<PlanTrabajoEntity> ActualizarPlanAsync(PlanTrabajoEntity plan)
        {
            _context.Entry(plan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<IEnumerable<PlanTrabajoEntity>> ObtenerPlanesPorLiderAsync(string liderClave)
        {
            var planes = await _context.PlanesTrabajos
                .Include(p => p.Dias)
                .Where(p => p.LiderClave == liderClave)
                .OrderByDescending(p => p.Semana)
                .ToListAsync();

            // Cargar clientes asignados para cada día de cada plan
            foreach (var plan in planes)
            {
                if (plan.Dias != null)
                {
                    foreach (var dia in plan.Dias)
                    {
                        await _context.Entry(dia)
                            .Collection(d => d.ClientesAsignados)
                            .LoadAsync();
                    }
                }
            }

            return planes;
        }

        public async Task<bool> ExistePlanAsync(string liderClave, int semana)
        {
            return await _context.PlanesTrabajos
                .AnyAsync(p => p.LiderClave == liderClave && p.Semana == semana);
        }
    }
}