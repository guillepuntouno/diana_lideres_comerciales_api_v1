using System;

namespace DIANA.Maestros.Models.DTOs
{
    public class VisitaCreateDto
    {
        public string VisitaId { get; set; }
        public string LiderClave { get; set; }
        public string ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string PlanId { get; set; }
        public string Dia { get; set; }
        public string CheckInTimestamp { get; set; }
        public string CheckInComentarios { get; set; }
        public double? CheckInLatitud { get; set; }
        public double? CheckInLongitud { get; set; }
        public double? CheckInPrecision { get; set; }
        public string CheckInDireccion { get; set; }
    }

    public class CheckInDto
    {
        public string Timestamp { get; set; }
        public string Comentarios { get; set; }
        public UbicacionDto Ubicacion { get; set; }
    }

    public class CheckOutDto
    {
        public string Timestamp { get; set; }
        public string Comentarios { get; set; }
        public int DuracionMinutos { get; set; }
        public UbicacionDto Ubicacion { get; set; }
    }

    public class UbicacionDto
    {
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double Precision { get; set; }
        public string Direccion { get; set; }
    }

    public class FormularioDto
    {
        public bool PoseeExhibidorAdecuado { get; set; }
        public int CantidadExhibidores { get; set; }
        public bool PrimeraPosition { get; set; }
        public bool Planograma { get; set; }
        public bool PortafolioFoco { get; set; }
        public bool Anclaje { get; set; }
        public bool Ristras { get; set; }
        public bool Max { get; set; }
        public bool Familiar { get; set; }
        public bool Dulce { get; set; }
        public bool Galleta { get; set; }
        public string Retroalimentacion { get; set; }
        public string Reconocimiento { get; set; }
    }
}