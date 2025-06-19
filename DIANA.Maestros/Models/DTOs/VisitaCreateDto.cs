using System;

namespace DIANA.Maestros.Models.DTOs
{
    public class VisitaCreateDto
    {
        public string ClaveVisita { get; set; }
        public string LiderClave { get; set; }
        public string ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string PlanId { get; set; }
        public string Dia { get; set; }
        public CheckInDto CheckIn { get; set; }
    }

    public class CheckInDto
    {
        public string Timestamp { get; set; }
        public string Comentarios { get; set; }
        public UbicacionDto Ubicacion { get; set; }
    }

    public class UbicacionDto
    {
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double Precision { get; set; }
        public string Direccion { get; set; }
    }
}