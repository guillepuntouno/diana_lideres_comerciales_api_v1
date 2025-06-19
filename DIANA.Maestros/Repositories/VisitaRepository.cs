using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DIANA.Maestros.Data;
using DIANA.Maestros.Models.Entities;

namespace DIANA.Maestros.Repositories
{
    public class VisitaRepository : IVisitaRepository
    {
        private readonly DianaDbContext _context;

        public VisitaRepository(DianaDbContext context)
        {
            _context = context;
        }

        public async Task<VisitaEntity> CrearVisitaAsync(VisitaEntity visita)
        {
            _context.Visitas.Add(visita);
            await _context.SaveChangesAsync();
            return visita;
        }

        public async Task<VisitaEntity> ObtenerVisitaPorIdAsync(string visitaId)
        {
            return await _context.Visitas
                // TEMPORALMENTE COMENTADO - .Include(v => v.Formulario)
                // TEMPORALMENTE COMENTADO - .Include(v => v.Formulario.Compromisos)
                .FirstOrDefaultAsync(v => v.VisitaId == visitaId);
        }

        public async Task<VisitaEntity> ActualizarVisitaAsync(VisitaEntity visita)
        {
            _context.Entry(visita).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return visita;
        }

        public async Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorLiderAsync(string liderClave)
        {
            return await _context.Visitas
                // TEMPORALMENTE COMENTADO - .Include(v => v.Formulario)
                .Where(v => v.LiderClave == liderClave)
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorDiaAsync(string liderClave, string dia)
        {
            return await _context.Visitas
                // TEMPORALMENTE COMENTADO - .Include(v => v.Formulario)
                .Where(v => v.LiderClave == liderClave && v.Dia == dia)
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();
        }

        public async Task<bool> CancelarVisitaAsync(string visitaId, string motivo)
        {
            var visita = await _context.Visitas.FindAsync(visitaId);
            if (visita == null) return false;

            visita.Estatus = "cancelada";
            visita.MotivoCancelacion = motivo;
            visita.FechaCancelacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> ObtenerResumenVisitasAsync(string liderClave, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var inicio = fechaInicio ?? DateTime.Now.AddDays(-30);
            var fin = fechaFin ?? DateTime.Now;

            var visitas = await _context.Visitas
                .Where(v => v.LiderClave == liderClave && 
                       v.FechaCreacion >= inicio && 
                       v.FechaCreacion <= fin)
                .ToListAsync();

            var resumen = new
            {
                TotalVisitas = visitas.Count,
                Completadas = visitas.Count(v => v.Estatus == "completada"),
                EnProceso = visitas.Count(v => v.Estatus == "en_proceso"),
                Canceladas = visitas.Count(v => v.Estatus == "cancelada"),
                FechaInicio = inicio.ToString("yyyy-MM-dd"),
                FechaFin = fin.ToString("yyyy-MM-dd"),
                Visitas = visitas
            };

            return resumen;
        }
    }
}