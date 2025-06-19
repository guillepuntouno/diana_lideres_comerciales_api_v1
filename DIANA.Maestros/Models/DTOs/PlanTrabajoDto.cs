using System;
using System.Collections.Generic;

namespace DIANA.Maestros.Models.DTOs
{
    public class PlanTrabajoCreateDto
    {
        public string LiderClave { get; set; }
    }

    public class PlanTrabajoResponseDto
    {
        public string PlanId { get; set; }
        public string LiderClave { get; set; }
        public int Semana { get; set; }
        public DateTime FechaCreacion { get; set; }
        public PlanSemanaDto Datos { get; set; }
    }

    public class PlanTrabajoUpdateDto
    {
        public string PlanId { get; set; }
        public Dictionary<string, PlanDiaDto> Datos { get; set; }
    }

    public class PlanSemanaDto
    {
        public Dictionary<string, PlanDiaDto> Semana { get; set; }
    }

    public class PlanDiaDto
    {
        public string Dia { get; set; }
        public string Objetivo { get; set; }
        public string Tipo { get; set; }
        public string CentroDistribucion { get; set; }
        public string RutaId { get; set; }
        public string RutaNombre { get; set; }
        public string TipoActividad { get; set; }
        public string Comentario { get; set; }
        public List<ClienteAsignadoDto> ClientesAsignados { get; set; }
        public bool Completado { get; set; }
    }

    public class ClienteAsignadoDto
    {
        public string ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteDireccion { get; set; }
        public string ClienteTipo { get; set; }
        public bool Visitado { get; set; }
        public DateTime? FechaVisita { get; set; }
    }
}