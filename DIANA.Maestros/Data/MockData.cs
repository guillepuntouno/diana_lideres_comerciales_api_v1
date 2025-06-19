using DIANA.Maestros.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace DIANA.Maestros.Data
{
    //public static class MockData
    //{
    //    public static List<LiderComercial> ObtenerLideres()
    //    {
    //        return new List<LiderComercial>
    //        {
    //            new LiderComercial
    //            {
    //                Clave = "LID001",
    //                Nombre = "LIDER CS - GRUPO A",
    //                Pais = "El Salvador",
    //                CentroDistribucion = "Centro de Servicio",
    //                Rutas = new List<Ruta>
    //                {
    //                    new Ruta
    //                    {
    //                        Nombre = "RUTACSD02",
    //                        Asesor = "Doris Gómez",
    //                        Negocios = new List<Negocio>
    //                        {
    //                            new Negocio
    //                            {
    //                                Clave = "1002-1",
    //                                Nombre = "Tienda La Esquinita",
    //                                Canal = "Canal D1",
    //                                Clasificacion = "A",
    //                                Exhibidor = "Exhibidor 01, Exhibido 02"
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        };
    //    }
    //}


    public static class MockData
    {
        public static List<LiderComercial> ObtenerLideres()
        {
            var ruta = HostingEnvironment.MapPath("~/App_Data/lideres_comerciales_completo.json");

            if (!File.Exists(ruta))
                return new List<LiderComercial>();

            var json = File.ReadAllText(ruta);
            return JsonConvert.DeserializeObject<List<LiderComercial>>(json);
        }
    }

}