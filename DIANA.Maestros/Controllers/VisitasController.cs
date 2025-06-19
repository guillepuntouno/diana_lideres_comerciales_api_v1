using DIANA.Maestros.Data;
using DIANA.Maestros.Models;
using DIANA.Maestros.Models.DTOs;
using DIANA.Maestros.Models.Entities;
using DIANA.Maestros.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace DIANA.Maestros.Controllers
{
    public class VisitasController : ApiController
    {
        private readonly string basePath = HostingEnvironment.MapPath("~/App_Data/visitas/");
        private readonly DianaDbContext _context;
        private readonly IVisitaRepository _visitaRepository;

        // Constructor sin parámetros para compatibilidad
        public VisitasController()
        {
            _context = new DianaDbContext();
            _visitaRepository = new VisitaRepository(_context);
        }

        // Constructor con inyección de dependencias
        public VisitasController(IVisitaRepository visitaRepository)
        {
            _visitaRepository = visitaRepository;
        }

        // VERSION 1 - COMENTADA - Usando JSON
        /*
        /// <summary>
        /// Crear una nueva visita con check-in inicial
        /// POST /api/visitas
        /// </summary>
        [HttpPost]
        [Route("api/visitas")]
        public IHttpActionResult CrearVisita([FromBody] dynamic body)
        {
            try
            {
                // Extraer datos del check-in
                string claveVisita = (string)body.claveVisita;
                string liderClave = (string)body.liderClave;
                string clienteId = (string)body.clienteId;

                if (string.IsNullOrEmpty(claveVisita))
                    return BadRequest("Falta la claveVisita.");

                if (string.IsNullOrEmpty(liderClave))
                    return BadRequest("Falta la liderClave.");

                if (string.IsNullOrEmpty(clienteId))
                    return BadRequest("Falta el clienteId.");

                string fileName = $"{claveVisita}.json";
                string path = Path.Combine(basePath, fileName);

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                if (File.Exists(path))
                    return Ok(new { mensaje = "Ya existe una visita con esta clave." });

                // Crear estructura inicial de la visita
                var nuevaVisita = new
                {
                    VisitaId = claveVisita,
                    LiderClave = liderClave,
                    ClienteId = clienteId,
                    ClienteNombre = (string)body.clienteNombre ?? "",
                    PlanId = (string)body.planId ?? "",
                    Dia = (string)body.dia ?? "",
                    FechaCreacion = DateTime.Now,
                    CheckIn = new
                    {
                        Timestamp = (string)body.checkIn?.timestamp ?? DateTime.Now.ToString("o"),
                        Comentarios = (string)body.checkIn?.comentarios ?? "",
                        Ubicacion = new
                        {
                            Latitud = (double?)body.checkIn?.ubicacion?.latitud ?? 0.0,
                            Longitud = (double?)body.checkIn?.ubicacion?.longitud ?? 0.0,
                            Precision = (double?)body.checkIn?.ubicacion?.precision ?? 0.0,
                            Direccion = (string)body.checkIn?.ubicacion?.direccion ?? ""
                        }
                    },
                    CheckOut = (object)null, // Se llenará al finalizar
                    Formularios = new { }, // Se llenará con los formularios dinámicos
                    Estatus = "en_proceso" // en_proceso, completada, cancelada
                };

                File.WriteAllText(path, JsonConvert.SerializeObject(nuevaVisita, Formatting.Indented));

                return Json(nuevaVisita);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Obtener una visita específica por clave
        /// GET /api/visitas/{claveVisita}
        /// </summary>
        [HttpGet]
        [Route("api/visitas/{claveVisita}")]
        public IHttpActionResult ObtenerVisita(string claveVisita)
        {
            try
            {
                string path = Path.Combine(basePath, $"{claveVisita}.json");
                if (!File.Exists(path))
                    return Json(new { mensaje = "No existe visita con esta clave." });

                var contenido = File.ReadAllText(path);
                var visitaJson = JObject.Parse(contenido);

                return Json(visitaJson);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Actualizar visita con formularios dinámicos
        /// PUT /api/visitas/{claveVisita}/formularios
        /// </summary>
        [HttpPut]
        [Route("api/visitas/{claveVisita}/formularios")]
        public IHttpActionResult ActualizarFormularios(string claveVisita, [FromBody] dynamic body)
        {
            try
            {
                string path = Path.Combine(basePath, $"{claveVisita}.json");
                if (!File.Exists(path))
                    return BadRequest("No existe visita con esta clave.");

                var contenido = File.ReadAllText(path);
                var visita = JObject.Parse(contenido);

                // Asegurar que existe la estructura de formularios
                if (visita["Formularios"] == null)
                    visita["Formularios"] = new JObject();

                var formularios = (JObject)visita["Formularios"];

                // Actualizar o agregar formularios
                foreach (var formulario in body.formularios)
                {
                    formularios[formulario.Name] = formulario.Value;
                }

                // Actualizar timestamp de modificación
                visita["FechaModificacion"] = DateTime.Now;

                File.WriteAllText(path, visita.ToString(Formatting.Indented));

                return Json(visita);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Finalizar visita con check-out
        /// PUT /api/visitas/{claveVisita}/checkout
        /// </summary>
        [HttpPut]
        [Route("api/visitas/{claveVisita}/checkout")]
        public IHttpActionResult FinalizarVisita(string claveVisita, [FromBody] dynamic body)
        {
            try
            {
                string path = Path.Combine(basePath, $"{claveVisita}.json");
                if (!File.Exists(path))
                    return BadRequest("No existe visita con esta clave.");

                var contenido = File.ReadAllText(path);
                var visita = JObject.Parse(contenido);

                // Agregar check-out
                visita["CheckOut"] = JObject.FromObject(new
                {
                    Timestamp = (string)body.timestamp ?? DateTime.Now.ToString("o"),
                    Comentarios = (string)body.comentarios ?? "",
                    Ubicacion = new
                    {
                        Latitud = (double?)body.ubicacion?.latitud ?? 0.0,
                        Longitud = (double?)body.ubicacion?.longitud ?? 0.0,
                        Precision = (double?)body.ubicacion?.precision ?? 0.0,
                        Direccion = (string)body.ubicacion?.direccion ?? ""
                    },
                    DuracionMinutos = (int?)body.duracionMinutos ?? 0
                });

                // Cambiar estatus a completada
                visita["Estatus"] = "completada";
                visita["FechaFinalizacion"] = DateTime.Now;

                File.WriteAllText(path, visita.ToString(Formatting.Indented));

                return Json(visita);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Obtener todas las visitas de un líder
        /// GET /api/visitas/lider/{liderClave}
        /// </summary>
        [HttpGet]
        [Route("api/visitas/lider/{liderClave}")]
        public IHttpActionResult ObtenerVisitasPorLider(string liderClave)
        {
            try
            {
                if (!Directory.Exists(basePath))
                    return Json(new { mensaje = "No hay visitas disponibles." });

                var archivos = Directory.GetFiles(basePath, $"{liderClave}_*.json");
                var visitas = new List<JObject>();

                foreach (var archivo in archivos)
                {
                    var contenido = File.ReadAllText(archivo);
                    var visita = JObject.Parse(contenido);
                    visitas.Add(visita);
                }

                return Json(visitas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Obtener visitas por día específico
        /// GET /api/visitas/lider/{liderClave}/dia/{dia}
        /// </summary>
        [HttpGet]
        [Route("api/visitas/lider/{liderClave}/dia/{dia}")]
        public IHttpActionResult ObtenerVisitasPorDia(string liderClave, string dia)
        {
            try
            {
                if (!Directory.Exists(basePath))
                    return Json(new { mensaje = "No hay visitas disponibles." });

                var archivos = Directory.GetFiles(basePath, $"{liderClave}_*_{dia}_*.json");
                var visitas = new List<JObject>();

                foreach (var archivo in archivos)
                {
                    var contenido = File.ReadAllText(archivo);
                    var visita = JObject.Parse(contenido);
                    visitas.Add(visita);
                }

                return Json(visitas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Cancelar una visita
        /// PUT /api/visitas/{claveVisita}/cancelar
        /// </summary>
        [HttpPut]
        [Route("api/visitas/{claveVisita}/cancelar")]
        public IHttpActionResult CancelarVisita(string claveVisita, [FromBody] dynamic body)
        {
            try
            {
                string path = Path.Combine(basePath, $"{claveVisita}.json");
                if (!File.Exists(path))
                    return BadRequest("No existe visita con esta clave.");

                var contenido = File.ReadAllText(path);
                var visita = JObject.Parse(contenido);

                // Cambiar estatus y agregar motivo
                visita["Estatus"] = "cancelada";
                visita["MotivoCancelacion"] = (string)body.motivo ?? "Sin motivo especificado";
                visita["FechaCancelacion"] = DateTime.Now;

                File.WriteAllText(path, visita.ToString(Formatting.Indented));

                return Json(visita);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Obtener resumen de visitas por rango de fechas
        /// GET /api/visitas/lider/{liderClave}/resumen?fechaInicio={fecha}&fechaFin={fecha}
        /// </summary>
        [HttpGet]
        [Route("api/visitas/lider/{liderClave}/resumen")]
        public IHttpActionResult ObtenerResumenVisitas(string liderClave, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                if (!Directory.Exists(basePath))
                    return Json(new { mensaje = "No hay visitas disponibles." });

                var archivos = Directory.GetFiles(basePath, $"{liderClave}_*.json");
                var visitasEnRango = new List<JObject>();

                DateTime inicio = fechaInicio ?? DateTime.Now.AddDays(-30);
                DateTime fin = fechaFin ?? DateTime.Now;

                foreach (var archivo in archivos)
                {
                    var contenido = File.ReadAllText(archivo);
                    var visita = JObject.Parse(contenido);

                    DateTime fechaCreacion = (DateTime)visita["FechaCreacion"];
                    if (fechaCreacion >= inicio && fechaCreacion <= fin)
                    {
                        visitasEnRango.Add(visita);
                    }
                }

                // Calcular estadísticas
                var resumen = new
                {
                    TotalVisitas = visitasEnRango.Count,
                    Completadas = visitasEnRango.Count(v => (string)v["Estatus"] == "completada"),
                    EnProceso = visitasEnRango.Count(v => (string)v["Estatus"] == "en_proceso"),
                    Canceladas = visitasEnRango.Count(v => (string)v["Estatus"] == "cancelada"),
                    FechaInicio = inicio.ToString("yyyy-MM-dd"),
                    FechaFin = fin.ToString("yyyy-MM-dd"),
                    Visitas = visitasEnRango
                };

                return Json(resumen);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        */

        // VERSION 2 - Usando Entity Framework y Base de Datos SQL

        /// <summary>
        /// Crear una nueva visita con check-in inicial
        /// POST /api/visitas
        /// </summary>
        [HttpPost]
        [Route("api/visitas")]
        public async Task<IHttpActionResult> CrearVisita([FromBody] VisitaCreateDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.VisitaId))
                    return BadRequest("Falta la VisitaId.");

                if (string.IsNullOrEmpty(dto.LiderClave))
                    return BadRequest("Falta la LiderClave.");

                if (string.IsNullOrEmpty(dto.ClienteId))
                    return BadRequest("Falta el ClienteId.");

                // Verificar si ya existe una visita con esta clave
                if (await _visitaRepository.ExisteVisitaAsync(dto.VisitaId))
                {
                    return Ok(new { mensaje = "Ya existe una visita con esta clave." });
                }

                var nuevaVisita = new VisitaEntity
                {
                    VisitaId = dto.VisitaId,
                    LiderClave = dto.LiderClave,
                    ClienteId = dto.ClienteId,
                    ClienteNombre = dto.ClienteNombre ?? "",
                    PlanId = dto.PlanId ?? "",
                    Dia = dto.Dia ?? "",
                    FechaCreacion = DateTime.Now,
                    CheckInTimestamp = dto.CheckInTimestamp ?? DateTime.Now.ToString("o"),
                    CheckInComentarios = dto.CheckInComentarios ?? "",
                    CheckInLatitud = dto.CheckInLatitud ?? 0.0,
                    CheckInLongitud = dto.CheckInLongitud ?? 0.0,
                    CheckInPrecision = dto.CheckInPrecision ?? 0.0,
                    CheckInDireccion = dto.CheckInDireccion ?? "",
                    Estatus = "en_proceso"
                };

                var visitaCreada = await _visitaRepository.CrearVisitaAsync(nuevaVisita);

                // Mapear a DTO de respuesta
                var respuesta = new VisitaResponseDto
                {
                    VisitaId = visitaCreada.VisitaId,
                    LiderClave = visitaCreada.LiderClave,
                    ClienteId = visitaCreada.ClienteId,
                    ClienteNombre = visitaCreada.ClienteNombre,
                    PlanId = visitaCreada.PlanId,
                    Dia = visitaCreada.Dia,
                    FechaCreacion = visitaCreada.FechaCreacion,
                    Estatus = visitaCreada.Estatus,
                    CheckIn = new CheckInDto
                    {
                        Timestamp = visitaCreada.CheckInTimestamp,
                        Comentarios = visitaCreada.CheckInComentarios,
                        Ubicacion = new UbicacionDto
                        {
                            Latitud = visitaCreada.CheckInLatitud ?? 0.0,
                            Longitud = visitaCreada.CheckInLongitud ?? 0.0,
                            Precision = visitaCreada.CheckInPrecision ?? 0.0,
                            Direccion = visitaCreada.CheckInDireccion
                        }
                    }
                };

                return Json(respuesta);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al crear la visita.", ex));
            }
        }

        /// <summary>
        /// Obtener una visita específica por clave
        /// GET /api/visitas/{claveVisita}
        /// </summary>
        [HttpGet]
        [Route("api/visitas/{claveVisita}")]
        public async Task<IHttpActionResult> ObtenerVisita(string claveVisita)
        {
            try
            {
                var visita = await _visitaRepository.ObtenerVisitaPorClaveAsync(claveVisita);
                
                if (visita == null)
                {
                    return Json(new { mensaje = "No existe visita con esta clave." });
                }

                // Mapear Entity a DTO completo
                var visitaDto = new VisitaResponseDto
                {
                    VisitaId = visita.VisitaId,
                    LiderClave = visita.LiderClave,
                    ClienteId = visita.ClienteId,
                    ClienteNombre = visita.ClienteNombre,
                    PlanId = visita.PlanId,
                    Dia = visita.Dia,
                    FechaCreacion = visita.FechaCreacion,
                    Estatus = visita.Estatus,
                    FechaModificacion = visita.FechaModificacion,
                    FechaFinalizacion = visita.FechaFinalizacion,
                    CheckIn = new CheckInDto
                    {
                        Timestamp = visita.CheckInTimestamp,
                        Comentarios = visita.CheckInComentarios,
                        Ubicacion = new UbicacionDto
                        {
                            Latitud = visita.CheckInLatitud ?? 0.0,
                            Longitud = visita.CheckInLongitud ?? 0.0,
                            Precision = visita.CheckInPrecision ?? 0.0,
                            Direccion = visita.CheckInDireccion
                        }
                    },
                    CheckOut = !string.IsNullOrEmpty(visita.CheckOutTimestamp) ? new CheckOutDto
                    {
                        Timestamp = visita.CheckOutTimestamp,
                        Comentarios = visita.CheckOutComentarios,
                        DuracionMinutos = visita.DuracionMinutos ?? 0,
                        Ubicacion = new UbicacionDto
                        {
                            Latitud = visita.CheckOutLatitud ?? 0.0,
                            Longitud = visita.CheckOutLongitud ?? 0.0,
                            Precision = visita.CheckOutPrecision ?? 0.0,
                            Direccion = visita.CheckOutDireccion
                        }
                    } : null,
                    Formularios = visita.Formulario != null ? new FormularioDto
                    {
                        PoseeExhibidorAdecuado = visita.Formulario.PoseeExhibidorAdecuado,
                        CantidadExhibidores = visita.Formulario.CantidadExhibidores,
                        PrimeraPosition = visita.Formulario.PrimeraPosition,
                        Planograma = visita.Formulario.Planograma,
                        PortafolioFoco = visita.Formulario.PortafolioFoco,
                        Anclaje = visita.Formulario.Anclaje,
                        Ristras = visita.Formulario.Ristras,
                        Max = visita.Formulario.Max,
                        Familiar = visita.Formulario.Familiar,
                        Dulce = visita.Formulario.Dulce,
                        Galleta = visita.Formulario.Galleta,
                        Retroalimentacion = visita.Formulario.Retroalimentacion,
                        Reconocimiento = visita.Formulario.Reconocimiento
                    } : null
                };

                return Json(visitaDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener la visita.", ex));
            }
        }

        /// <summary>
        /// Actualizar visita con formularios dinámicos
        /// PUT /api/visitas/{claveVisita}/formularios
        /// </summary>
        [HttpPut]
        [Route("api/visitas/{claveVisita}/formularios")]
        public async Task<IHttpActionResult> ActualizarFormularios(string claveVisita, [FromBody] FormularioDto formularioDto)
        {
            try
            {
                var visita = await _visitaRepository.ObtenerVisitaPorClaveAsync(claveVisita);
                if (visita == null)
                {
                    return BadRequest("No existe visita con esta clave.");
                }

                await _visitaRepository.ActualizarFormularioAsync(claveVisita, formularioDto);

                return Json(new { mensaje = "Formulario actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al actualizar el formulario.", ex));
            }
        }

        /// <summary>
        /// Finalizar visita con check-out
        /// PUT /api/visitas/{claveVisita}/checkout
        /// </summary>
        [HttpPut]
        [Route("api/visitas/{claveVisita}/checkout")]
        public async Task<IHttpActionResult> FinalizarVisita(string claveVisita, [FromBody] CheckOutDto checkOutDto)
        {
            try
            {
                var visita = await _visitaRepository.ObtenerVisitaPorClaveAsync(claveVisita);
                if (visita == null)
                {
                    return BadRequest("No existe visita con esta clave.");
                }

                await _visitaRepository.FinalizarVisitaAsync(claveVisita, checkOutDto);

                return Json(new { mensaje = "Visita finalizada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al finalizar la visita.", ex));
            }
        }

        /// <summary>
        /// Obtener todas las visitas de un líder
        /// GET /api/visitas/lider/{liderClave}
        /// </summary>
        [HttpGet]
        [Route("api/visitas/lider/{liderClave}")]
        public async Task<IHttpActionResult> ObtenerVisitasPorLider(string liderClave)
        {
            try
            {
                var visitas = await _visitaRepository.ObtenerVisitasPorLiderAsync(liderClave);

                if (!visitas.Any())
                {
                    return Json(new { mensaje = "No hay visitas disponibles." });
                }

                var visitasDto = visitas.Select(v => new VisitaResponseDto
                {
                    VisitaId = v.VisitaId,
                    LiderClave = v.LiderClave,
                    ClienteId = v.ClienteId,
                    ClienteNombre = v.ClienteNombre,
                    PlanId = v.PlanId,
                    Dia = v.Dia,
                    FechaCreacion = v.FechaCreacion,
                    Estatus = v.Estatus,
                    FechaModificacion = v.FechaModificacion,
                    FechaFinalizacion = v.FechaFinalizacion
                }).ToList();

                return Json(visitasDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener las visitas del líder.", ex));
            }
        }

        /// <summary>
        /// Obtener visitas por día específico
        /// GET /api/visitas/lider/{liderClave}/dia/{dia}
        /// </summary>
        [HttpGet]
        [Route("api/visitas/lider/{liderClave}/dia/{dia}")]
        public async Task<IHttpActionResult> ObtenerVisitasPorDia(string liderClave, string dia)
        {
            try
            {
                var visitas = await _visitaRepository.ObtenerVisitasPorLiderYDiaAsync(liderClave, dia);

                if (!visitas.Any())
                {
                    return Json(new { mensaje = "No hay visitas disponibles." });
                }

                var visitasDto = visitas.Select(v => new VisitaResponseDto
                {
                    VisitaId = v.VisitaId,
                    LiderClave = v.LiderClave,
                    ClienteId = v.ClienteId,
                    ClienteNombre = v.ClienteNombre,
                    PlanId = v.PlanId,
                    Dia = v.Dia,
                    FechaCreacion = v.FechaCreacion,
                    Estatus = v.Estatus,
                    FechaModificacion = v.FechaModificacion,
                    FechaFinalizacion = v.FechaFinalizacion
                }).ToList();

                return Json(visitasDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener las visitas por día.", ex));
            }
        }

        /// <summary>
        /// Cancelar una visita
        /// PUT /api/visitas/{claveVisita}/cancelar
        /// </summary>
        [HttpPut]
        [Route("api/visitas/{claveVisita}/cancelar")]
        public async Task<IHttpActionResult> CancelarVisita(string claveVisita, [FromBody] dynamic body)
        {
            try
            {
                var visita = await _visitaRepository.ObtenerVisitaPorClaveAsync(claveVisita);
                if (visita == null)
                {
                    return BadRequest("No existe visita con esta clave.");
                }

                string motivo = (string)body.motivo ?? "Sin motivo especificado";
                await _visitaRepository.CancelarVisitaAsync(claveVisita, motivo);

                return Json(new { mensaje = "Visita cancelada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al cancelar la visita.", ex));
            }
        }

        /// <summary>
        /// Obtener resumen de visitas por rango de fechas
        /// GET /api/visitas/lider/{liderClave}/resumen?fechaInicio={fecha}&fechaFin={fecha}
        /// </summary>
        [HttpGet]
        [Route("api/visitas/lider/{liderClave}/resumen")]
        public async Task<IHttpActionResult> ObtenerResumenVisitas(string liderClave, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                DateTime inicio = fechaInicio ?? DateTime.Now.AddDays(-30);
                DateTime fin = fechaFin ?? DateTime.Now;

                var visitas = await _visitaRepository.ObtenerVisitasPorRangoFechasAsync(liderClave, inicio, fin);

                var resumen = new
                {
                    TotalVisitas = visitas.Count(),
                    Completadas = visitas.Count(v => v.Estatus == "completada"),
                    EnProceso = visitas.Count(v => v.Estatus == "en_proceso"),
                    Canceladas = visitas.Count(v => v.Estatus == "cancelada"),
                    FechaInicio = inicio.ToString("yyyy-MM-dd"),
                    FechaFin = fin.ToString("yyyy-MM-dd"),
                    Visitas = visitas.Select(v => new VisitaResponseDto
                    {
                        VisitaId = v.VisitaId,
                        LiderClave = v.LiderClave,
                        ClienteId = v.ClienteId,
                        ClienteNombre = v.ClienteNombre,
                        PlanId = v.PlanId,
                        Dia = v.Dia,
                        FechaCreacion = v.FechaCreacion,
                        Estatus = v.Estatus,
                        FechaModificacion = v.FechaModificacion,
                        FechaFinalizacion = v.FechaFinalizacion
                    }).ToList()
                };

                return Json(resumen);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener el resumen de visitas.", ex));
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