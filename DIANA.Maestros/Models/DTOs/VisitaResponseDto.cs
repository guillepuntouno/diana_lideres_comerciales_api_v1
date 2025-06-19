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
        public CheckInDto CheckIn { get; set; }
        public CheckOutDto CheckOut { get; set; }
        public FormularioDto Formularios { get; set; }
        public string Estatus { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
    }

}