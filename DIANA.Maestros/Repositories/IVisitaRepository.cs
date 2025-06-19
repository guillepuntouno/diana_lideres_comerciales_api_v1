using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIANA.Maestros.Models.Entities;

namespace DIANA.Maestros.Repositories
{
    public interface IVisitaRepository
    {
        Task<VisitaEntity> CrearVisitaAsync(VisitaEntity visita);
        Task<VisitaEntity> ObtenerVisitaPorIdAsync(string visitaId);
        Task<VisitaEntity> ActualizarVisitaAsync(VisitaEntity visita);
        Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorLiderAsync(string liderClave);
        Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorDiaAsync(string liderClave, string dia);
        Task<bool> CancelarVisitaAsync(string visitaId, string motivo);
        Task<object> ObtenerResumenVisitasAsync(string liderClave, DateTime? fechaInicio, DateTime? fechaFin);
    }
}