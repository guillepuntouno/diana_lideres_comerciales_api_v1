using System.Collections.Generic;

namespace DIANA.Maestros.Models.DTOs
{
    public class LiderComercialResponseDto
    {
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string CentroDistribucion { get; set; }
        public List<RutaDto> Rutas { get; set; }
    }

    public class RutaDto
    {
        public string Nombre { get; set; }
        public string Asesor { get; set; }
        public List<NegocioDto> Negocios { get; set; }
    }

    public class NegocioDto
    {
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Canal { get; set; }
        public string Clasificacion { get; set; }
        public string Exhibidor { get; set; }
    }
}