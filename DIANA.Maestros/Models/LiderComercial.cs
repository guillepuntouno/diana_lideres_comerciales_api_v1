using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIANA.Maestros.Models
{
    public class LiderComercial
    {
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string CentroDistribucion { get; set; }
        public List<Ruta> Rutas { get; set; }
    }
}