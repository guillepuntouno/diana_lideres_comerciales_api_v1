using DIANA.Maestros.Data;
using DIANA.Maestros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}