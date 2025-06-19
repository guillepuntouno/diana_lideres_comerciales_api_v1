using System.Threading.Tasks;
using DIANA.Maestros.Models.Entities;

namespace DIANA.Maestros.Repositories
{
    public interface ILiderComercialRepository
    {
        Task<LiderComercialEntity> ObtenerPorClaveAsync(string clave);
    }
}