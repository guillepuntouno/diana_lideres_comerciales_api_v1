using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIANA.Maestros.Models
{
    public class Ruta
    {
        public string Nombre { get; set; }
        public string Asesor { get; set; }
        public List<Negocio> Negocios { get; set; }
    }
}