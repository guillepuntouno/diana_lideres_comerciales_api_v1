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
    public class PlanesController : ApiController
    {
        private readonly string basePath = HostingEnvironment.MapPath("~/App_Data/planes/");
        private readonly DianaDbContext _context;
        private readonly IPlanTrabajoRepository _planRepository;

        // Constructor sin parámetros para compatibilidad
        public PlanesController()
        {
            _context = new DianaDbContext();
            _planRepository = new PlanTrabajoRepository(_context);
        }

        // Constructor con inyección de dependencias
        public PlanesController(IPlanTrabajoRepository planRepository)
        {
            _planRepository = planRepository;
        }

        // VERSION 1 - COMENTADA - Usando JSON
        /*
        [HttpPost]
        [Route("api/planes")]
        public IHttpActionResult CrearPlan([FromBody] dynamic body)
        {
            try
            {
                string clave = (string)body.liderClave;

                var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                int semana = currentCulture.Calendar.GetWeekOfYear(
                    DateTime.Now,
                    System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Monday
                );

                string fileName = $"{clave}_SEM{semana}.json";
                string path = Path.Combine(basePath, fileName);

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                if (File.Exists(path))
                    return Ok(new { mensaje = "Ya existe un plan para esta semana." });

                var nuevoPlan = new PlanTrabajo
                {
                    PlanId = $"{clave}_SEM{semana}",
                    LiderClave = clave,
                    Semana = semana,
                    FechaCreacion = DateTime.Now,
                    Datos = new { } // Objeto vacío para inicializar
                };

                // Serializar manualmente para evitar problemas con dynamic
                var planJson = new
                {
                    PlanId = nuevoPlan.PlanId,
                    LiderClave = nuevoPlan.LiderClave,
                    Semana = nuevoPlan.Semana,
                    FechaCreacion = nuevoPlan.FechaCreacion,
                    datos = new { } // Campo vacío inicialmente
                };

                File.WriteAllText(path, JsonConvert.SerializeObject(planJson, Formatting.Indented));

                return Json(planJson); // Usar Json() en lugar de Ok() para forzar JSON
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        */

        // VERSION 2 - Usando Entity Framework y Base de Datos SQL
        [HttpPost]
        [Route("api/planes")]
        public async Task<IHttpActionResult> CrearPlan([FromBody] PlanTrabajoCreateDto dto)
        {
            try
            {
                var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                int semana = currentCulture.Calendar.GetWeekOfYear(
                    DateTime.Now,
                    System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Monday
                );

                // Verificar si ya existe un plan para esta semana
                if (await _planRepository.ExistePlanAsync(dto.LiderClave, semana))
                {
                    return Ok(new { mensaje = "Ya existe un plan para esta semana." });
                }

                var nuevoPlan = new PlanTrabajoEntity
                {
                    PlanId = $"{dto.LiderClave}_SEM{semana}",
                    LiderClave = dto.LiderClave,
                    Semana = semana,
                    Anio = DateTime.Now.Year,
                    FechaCreacion = DateTime.Now,
                    Estatus = "borrador"
                };

                var planCreado = await _planRepository.CrearPlanAsync(nuevoPlan);

                // Mapear a DTO de respuesta
                var respuesta = new PlanTrabajoResponseDto
                {
                    PlanId = planCreado.PlanId,
                    LiderClave = planCreado.LiderClave,
                    Semana = planCreado.Semana,
                    FechaCreacion = planCreado.FechaCreacion,
                    Datos = new PlanSemanaDto { Semana = new Dictionary<string, PlanDiaDto>() }
                };

                return Json(respuesta);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al crear el plan de trabajo.", ex));
            }
        }

        // VERSION 1 - COMENTADA
        /*
        [HttpGet]
        [Route("api/planes/{clave}/semana/{semana}")]
        public IHttpActionResult ObtenerPlan(string clave, int semana)
        {
            try
            {
                string path = Path.Combine(basePath, $"{clave}_SEM{semana}.json");
                if (!File.Exists(path))
                    return Json(new { mensaje = "No existe plan para esta semana." });

                var contenido = File.ReadAllText(path);

                // Deserializar como JObject para manejo más seguro
                var planJson = JObject.Parse(contenido);

                return Json(planJson); // Devolver el JSON tal como está
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        */

        // VERSION 2 - Usando Entity Framework
        [HttpGet]
        [Route("api/planes/{clave}/semana/{semana}")]
        public async Task<IHttpActionResult> ObtenerPlan(string clave, int semana)
        {
            try
            {
                var plan = await _planRepository.ObtenerPlanPorClaveYSemanaAsync(clave, semana);
                
                if (plan == null)
                {
                    return Json(new { mensaje = "No existe plan para esta semana." });
                }

                // Mapear Entity a DTO - COMPLETO
                var planDto = new PlanTrabajoResponseDto
                {
                    PlanId = plan.PlanId,
                    LiderClave = plan.LiderClave,
                    Semana = plan.Semana,
                    FechaCreacion = plan.FechaCreacion,
                    Datos = new PlanSemanaDto
                    {
                        Semana = plan.Dias?.ToDictionary(
                            d => d.Dia.ToLower(),
                            d => new PlanDiaDto
                            {
                                Dia = d.Dia,
                                Objetivo = d.Objetivo,
                                Tipo = d.Tipo,
                                CentroDistribucion = d.CentroDistribucion,
                                RutaId = d.RutaId,
                                RutaNombre = d.RutaNombre,
                                TipoActividad = d.TipoActividad,
                                Comentario = d.Comentario,
                                Completado = d.Completado,
                                ClientesAsignados = d.ClientesAsignados?.Select(c => new ClienteAsignadoDto
                                {
                                    ClienteId = c.ClienteId,
                                    ClienteNombre = c.ClienteNombre,
                                    ClienteDireccion = c.ClienteDireccion,
                                    ClienteTipo = c.ClienteTipo,
                                    Visitado = c.Visitado,
                                    FechaVisita = c.FechaVisita
                                }).ToList() ?? new List<ClienteAsignadoDto>()
                            }
                        ) ?? new Dictionary<string, PlanDiaDto>()
                    }
                };

                return Json(planDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener el plan de trabajo.", ex));
            }
        }

        // VERSION 1 - COMENTADA
        /*
        [HttpPut]
        [Route("api/planes")]
        public IHttpActionResult ActualizarPlanPorId([FromBody] dynamic body)
        {
            try
            {
                string planId = (string)body.planId;
                if (string.IsNullOrEmpty(planId))
                    return BadRequest("Falta el planId.");

                string path = Path.Combine(basePath, $"{planId}.json");
                if (!File.Exists(path))
                    return BadRequest("No existe un plan con ese planId.");

                var contenido = File.ReadAllText(path);
                var plan = JObject.Parse(contenido);

                // Asegurar que existe la estructura de datos
                if (plan["datos"] == null)
                    plan["datos"] = new JObject();

                var datos = (JObject)plan["datos"];

                if (datos["semana"] == null)
                    datos["semana"] = new JObject();

                var semana = (JObject)datos["semana"];

                // Actualizar los días
                foreach (var dia in body.datos)
                {
                    semana[dia.Name] = dia.Value;
                }

                File.WriteAllText(path, plan.ToString(Formatting.Indented));

                return Json(plan);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        */

        // VERSION 2 - Usando Entity Framework - SIMPLIFICADO
        [HttpPut]
        [Route("api/planes")]
        public async Task<IHttpActionResult> ActualizarPlanPorId([FromBody] dynamic body)
        {
            try
            {
                string planId = (string)body.planId;
                if (string.IsNullOrEmpty(planId))
                    return BadRequest("Falta el planId.");

                var plan = await _context.PlanesTrabajos
                    .FirstOrDefaultAsync(p => p.PlanId == planId);

                if (plan == null)
                    return BadRequest("No existe un plan con ese planId.");

                // Actualizar fecha de modificación
                plan.FechaActualizacion = DateTime.Now;
                plan.Estatus = "actualizado";

                await _context.SaveChangesAsync();

                // Devolver plan actualizado básico
                var planResponse = new PlanTrabajoResponseDto
                {
                    PlanId = plan.PlanId,
                    LiderClave = plan.LiderClave,
                    Semana = plan.Semana,
                    FechaCreacion = plan.FechaCreacion,
                    Datos = new PlanSemanaDto
                    {
                        Semana = new Dictionary<string, PlanDiaDto>()
                    }
                };

                return Json(planResponse);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al actualizar el plan de trabajo.", ex));
            }
        }

        // VERSION 1 - COMENTADA
        /*
        [HttpGet]
        [Route("api/planes/lider/{clave}")]
        public IHttpActionResult ObtenerPlanesPorLider(string clave)
        {
            try
            {
                if (!Directory.Exists(basePath))
                    return Json(new { mensaje = "No hay planes disponibles." });

                var archivos = Directory.GetFiles(basePath, $"{clave}_SEM*.json");
                var planes = new List<JObject>();

                foreach (var archivo in archivos)
                {
                    var contenido = File.ReadAllText(archivo);
                    var plan = JObject.Parse(contenido);
                    planes.Add(plan);
                }

                return Json(planes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        */

        // VERSION 2 - Usando Entity Framework
        [HttpGet]
        [Route("api/planes/lider/{clave}")]
        public async Task<IHttpActionResult> ObtenerPlanesPorLider(string clave)
        {
            try
            {
                var planes = await _planRepository.ObtenerPlanesPorLiderAsync(clave);

                if (!planes.Any())
                {
                    return Json(new { mensaje = "No hay planes disponibles." });
                }

                // Mapear entities a DTOs - COMPLETO
                var planesDto = planes.Select(plan => new PlanTrabajoResponseDto
                {
                    PlanId = plan.PlanId,
                    LiderClave = plan.LiderClave,
                    Semana = plan.Semana,
                    FechaCreacion = plan.FechaCreacion,
                    Datos = new PlanSemanaDto
                    {
                        Semana = plan.Dias?.ToDictionary(
                            d => d.Dia.ToLower(),
                            d => new PlanDiaDto
                            {
                                Dia = d.Dia,
                                Objetivo = d.Objetivo,
                                Tipo = d.Tipo,
                                CentroDistribucion = d.CentroDistribucion,
                                RutaId = d.RutaId,
                                RutaNombre = d.RutaNombre,
                                TipoActividad = d.TipoActividad,
                                Comentario = d.Comentario,
                                Completado = d.Completado,
                                ClientesAsignados = d.ClientesAsignados?.Select(c => new ClienteAsignadoDto
                                {
                                    ClienteId = c.ClienteId,
                                    ClienteNombre = c.ClienteNombre,
                                    ClienteDireccion = c.ClienteDireccion,
                                    ClienteTipo = c.ClienteTipo,
                                    Visitado = c.Visitado,
                                    FechaVisita = c.FechaVisita
                                }).ToList() ?? new List<ClienteAsignadoDto>()
                            }
                        ) ?? new Dictionary<string, PlanDiaDto>()
                    }
                }).ToList();

                return Json(planesDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener los planes del líder.", ex));
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
