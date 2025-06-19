using DIANA.Maestros.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;

namespace DIANA.Maestros.Controllers
{
    public class VisitasController : ApiController
    {
        private readonly string basePath = HostingEnvironment.MapPath("~/App_Data/visitas/");

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
    }
}