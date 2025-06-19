using DIANA.Maestros.Data;
using DIANA.Maestros.Models;
using DIANA.Maestros.Models.DTOs;
using DIANA.Maestros.Models.Entities;
using DIANA.Maestros.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;


namespace DIANA.Maestros.Controllers
{
    public class LideresController : ApiController
    {
        private static readonly List<LiderComercial> DatosMock = MockData.ObtenerLideres();
        private readonly DianaDbContext _context;
        private readonly ILiderComercialRepository _liderRepository;

        // Constructor sin parámetros para compatibilidad con la versión original
        public LideresController()
        {
            _context = new DianaDbContext();
            _liderRepository = new LiderComercialRepository(_context);
        }

        // Constructor con inyección de dependencias (para futuro uso con IoC)
        public LideresController(ILiderComercialRepository liderRepository)
        {
            _liderRepository = liderRepository;
        }

        // VERSION 1 - COMENTADA - Usando JSON
        /*
        [HttpGet]
        [Route("api/lideres/{clave}")]
        public IHttpActionResult GetPorClave(string clave)
        {
            var lider = DatosMock.FirstOrDefault(x => x.Clave == clave);
            if (lider == null)
            {
                return Ok(new { mensaje = "No se encontró información para la clave proporcionada." });
            }

            return Ok(lider);
        }
        */

        // VERSION 2 - Usando Entity Framework y Base de Datos SQL
        [HttpGet]
        [Route("api/lideres/{clave}")]
        public async Task<IHttpActionResult> GetPorClave(string clave)
        {
            try
            {
                var liderEntity = await _liderRepository.ObtenerPorClaveAsync(clave);
                
                if (liderEntity == null)
                {
                    return Ok(new { mensaje = "No se encontró información para la clave proporcionada." });
                }

                // Mapear Entity a DTO
                var liderDto = new LiderComercialResponseDto
                {
                    Clave = liderEntity.Clave,
                    Nombre = liderEntity.Nombre,
                    Pais = liderEntity.Pais,
                    CentroDistribucion = liderEntity.CentroDistribucion,
                    Rutas = liderEntity.Rutas?.Select(r => new RutaDto
                    {
                        Nombre = r.Nombre,
                        Asesor = r.Asesor,
                        Negocios = r.Negocios?.Select(n => new NegocioDto
                        {
                            Clave = n.Clave,
                            Nombre = n.Nombre,
                            Canal = n.Canal,
                            Clasificacion = n.Clasificacion,
                            Exhibidor = n.Exhibidor
                        }).ToList() ?? new List<NegocioDto>()
                    }).ToList() ?? new List<RutaDto>()
                };

                return Ok(liderDto);
            }
            catch (Exception ex)
            {
                // Log del error (implementar logging más adelante)
                return InternalServerError(new Exception("Error al obtener información del líder comercial.", ex));
            }
        }

        // Dispose pattern para liberar recursos
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}