using System;
using System.Collections.Generic;

namespace DIANA.Maestros.Models.DTOs
{
    public class VisitaResponseDto
    {
        public string VisitaId { get; set; }
        public string LiderClave { get; set; }
        public string ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string PlanId { get; set; }
        public string Dia { get; set; }
        public DateTime FechaCreacion { get; set; }
        public CheckInResponseDto CheckIn { get; set; }
        public CheckOutResponseDto CheckOut { get; set; }
        public FormulariosDto Formularios { get; set; }
        public string Estatus { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
    }

    public class CheckInResponseDto
    {
        public DateTime Timestamp { get; set; }
        public string Comentarios { get; set; }
        public UbicacionDto Ubicacion { get; set; }
    }

    public class CheckOutResponseDto
    {
        public DateTime Timestamp { get; set; }
        public string Comentarios { get; set; }
        public UbicacionDto Ubicacion { get; set; }
        public int DuracionMinutos { get; set; }
    }

    public class FormulariosDto
    {
        public EvaluacionDesarrolloCampoDto EvaluacionDesarrolloCampo { get; set; }
    }

    public class EvaluacionDesarrolloCampoDto
    {
        public Seccion1Dto Seccion1 { get; set; }
        public Seccion2Dto Seccion2 { get; set; }
        public Seccion3Dto Seccion3 { get; set; }
        public Seccion4Dto Seccion4 { get; set; }
        public Seccion5Dto Seccion5 { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Version { get; set; }
    }

    public class Seccion1Dto
    {
        public bool PoseeExhibidorAdecuado { get; set; }
        public string TipoExhibidorSeleccionado { get; set; }
        public string ModeloExhibidorSeleccionado { get; set; }
        public int CantidadExhibidores { get; set; }
        public List<string> ExhibidoresAsignados { get; set; }
    }

    public class Seccion2Dto
    {
        public bool PrimeraPosition { get; set; }
        public bool Planograma { get; set; }
        public bool PortafolioFoco { get; set; }
        public bool Anclaje { get; set; }
    }

    public class Seccion3Dto
    {
        public bool Ristras { get; set; }
        public bool Max { get; set; }
        public bool Familiar { get; set; }
        public bool Dulce { get; set; }
        public bool Galleta { get; set; }
    }

    public class Seccion4Dto
    {
        public List<CompromisoDto> Compromisos { get; set; }
        public string TipoCompromisoSeleccionado { get; set; }
        public string DetalleCompromisoSeleccionado { get; set; }
        public int CantidadCompromiso { get; set; }
        public DateTime? FechaCompromiso { get; set; }
    }

    public class CompromisoDto
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public string Detalle { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaFormateada { get; set; }
        public string ClienteId { get; set; }
        public string RutaId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Seccion5Dto
    {
        public string Retroalimentacion { get; set; }
        public string Reconocimiento { get; set; }
    }
}