using System.Collections.Generic;
using System.Threading.Tasks;
using DIANA.Maestros.Models.Entities;

namespace DIANA.Maestros.Repositories
{
    public interface IPlanTrabajoRepository
    {
        Task<PlanTrabajoEntity> CrearPlanAsync(PlanTrabajoEntity plan);
        Task<PlanTrabajoEntity> ObtenerPlanPorClaveYSemanaAsync(string liderClave, int semana);
        Task<PlanTrabajoEntity> ActualizarPlanAsync(PlanTrabajoEntity plan);
        Task<IEnumerable<PlanTrabajoEntity>> ObtenerPlanesPorLiderAsync(string liderClave);
        Task<bool> ExistePlanAsync(string liderClave, int semana);
    }
}