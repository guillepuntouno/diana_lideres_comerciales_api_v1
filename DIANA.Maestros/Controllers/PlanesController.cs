using DIANA.Maestros.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Http;

namespace DIANA.Maestros.Controllers
{
    public class PlanesController : ApiController
    {
        private readonly string basePath = HostingEnvironment.MapPath("~/App_Data/planes/");

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
    }
}
