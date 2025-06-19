using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DIANA.Maestros.Data;
using DIANA.Maestros.Models.DTOs;
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

        public async Task<VisitaEntity> ObtenerVisitaPorClaveAsync(string visitaId)
        {
            var visita = await _context.Visitas
                .Include(v => v.Formulario)
                .FirstOrDefaultAsync(v => v.VisitaId == visitaId);

            return visita;
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
                .Include(v => v.Formulario)
                .Where(v => v.LiderClave == liderClave)
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorLiderYDiaAsync(string liderClave, string dia)
        {
            return await _context.Visitas
                .Include(v => v.Formulario)
                .Where(v => v.LiderClave == liderClave && v.Dia == dia)
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<VisitaEntity>> ObtenerVisitasPorRangoFechasAsync(string liderClave, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Visitas
                .Include(v => v.Formulario)
                .Where(v => v.LiderClave == liderClave && 
                       v.FechaCreacion >= fechaInicio && 
                       v.FechaCreacion <= fechaFin)
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();
        }

        public async Task<bool> ExisteVisitaAsync(string visitaId)
        {
            return await _context.Visitas.AnyAsync(v => v.VisitaId == visitaId);
        }

        public async Task ActualizarFormularioAsync(string visitaId, FormularioDto formularioDto)
        {
            var visita = await _context.Visitas
                .Include(v => v.Formulario)
                .FirstOrDefaultAsync(v => v.VisitaId == visitaId);

            if (visita == null) return;

            if (visita.Formulario == null)
            {
                visita.Formulario = new VisitaFormularioEntity
                {
                    VisitaId = visitaId
                };
                _context.VisitaFormularios.Add(visita.Formulario);
            }

            // Actualizar formulario
            visita.Formulario.PoseeExhibidorAdecuado = formularioDto.PoseeExhibidorAdecuado;
            visita.Formulario.CantidadExhibidores = formularioDto.CantidadExhibidores;
            visita.Formulario.PrimeraPosition = formularioDto.PrimeraPosition;
            visita.Formulario.Planograma = formularioDto.Planograma;
            visita.Formulario.PortafolioFoco = formularioDto.PortafolioFoco;
            visita.Formulario.Anclaje = formularioDto.Anclaje;
            visita.Formulario.Ristras = formularioDto.Ristras;
            visita.Formulario.Max = formularioDto.Max;
            visita.Formulario.Familiar = formularioDto.Familiar;
            visita.Formulario.Dulce = formularioDto.Dulce;
            visita.Formulario.Galleta = formularioDto.Galleta;
            visita.Formulario.Retroalimentacion = formularioDto.Retroalimentacion;
            visita.Formulario.Reconocimiento = formularioDto.Reconocimiento;
            visita.Formulario.FechaActualizacion = DateTime.Now;

            // Actualizar visita
            visita.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task FinalizarVisitaAsync(string visitaId, CheckOutDto checkOutDto)
        {
            var visita = await _context.Visitas.FirstOrDefaultAsync(v => v.VisitaId == visitaId);
            if (visita == null) return;

            visita.CheckOutTimestamp = checkOutDto.Timestamp ?? DateTime.Now.ToString("o");
            visita.CheckOutComentarios = checkOutDto.Comentarios ?? "";
            visita.CheckOutLatitud = checkOutDto.Ubicacion?.Latitud ?? 0.0;
            visita.CheckOutLongitud = checkOutDto.Ubicacion?.Longitud ?? 0.0;
            visita.CheckOutPrecision = checkOutDto.Ubicacion?.Precision ?? 0.0;
            visita.CheckOutDireccion = checkOutDto.Ubicacion?.Direccion ?? "";
            visita.DuracionMinutos = checkOutDto.DuracionMinutos;
            visita.Estatus = "completada";
            visita.FechaFinalizacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task CancelarVisitaAsync(string visitaId, string motivo)
        {
            var visita = await _context.Visitas.FirstOrDefaultAsync(v => v.VisitaId == visitaId);
            if (visita == null) return;

            visita.Estatus = "cancelada";
            visita.MotivoCancelacion = motivo;
            visita.FechaCancelacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}