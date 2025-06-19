using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIANA.Maestros.Models.DTOs;
using DIANA.Maestros.Models.Entities;

namespace DIANA.Maestros.Repositories
{
    public interface IVisitaRepository
    {
        Task<VisitaEntity> CrearVisitaAsync(VisitaEntity visita);
        Task<VisitaEntity> ObtenerVisitaPorClaveAsync(string visitaId);
        Task<VisitaEntity> ActualizarVisitaAsync(VisitaEntity visita);
        Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorLiderAsync(string liderClave);
        Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorLiderYDiaAsync(string liderClave, string dia);
        Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorRangoFechasAsync(string liderClave, DateTime fechaInicio, DateTime fechaFin);
        Task<bool> ExisteVisitaAsync(string visitaId);
        Task ActualizarFormularioAsync(string visitaId, FormularioDto formularioDto);
        Task FinalizarVisitaAsync(string visitaId, CheckOutDto checkOutDto);
        Task CancelarVisitaAsync(string visitaId, string motivo);
    }
}