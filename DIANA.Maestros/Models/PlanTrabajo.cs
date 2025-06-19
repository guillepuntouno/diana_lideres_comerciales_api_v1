using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIANA.Maestros.Models
{
    public class PlanTrabajo
    {
        public string PlanId { get; set; } // Ej: LID001_SEM23
        public string LiderClave { get; set; }
        public int Semana { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Cambiar de dynamic a object y agregar JsonIgnore para la serialización por defecto
        [JsonIgnore]
        public dynamic Datos { get; set; } // Mantener para compatibilidad

        // Agregar una propiedad string para la serialización
        [JsonProperty("datos")]
        public string DatosJson
        {
            get
            {
                return Datos != null ? JsonConvert.SerializeObject(Datos) : "{}";
            }
            set
            {
                Datos = !string.IsNullOrEmpty(value) ? JsonConvert.DeserializeObject(value) : new { };
            }
        }
    }
}