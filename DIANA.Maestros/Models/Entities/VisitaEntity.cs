using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("Visitas")]
    public class VisitaEntity
    {
        [Key]
        [StringLength(100)]
        public string VisitaId { get; set; }

        [Required]
        [StringLength(20)]
        public string LiderClave { get; set; }

        [Required]
        [StringLength(20)]
        public string ClienteId { get; set; }

        [StringLength(100)]
        public string ClienteNombre { get; set; }

        [StringLength(50)]
        public string PlanId { get; set; }

        [StringLength(20)]
        public string Dia { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Check-in data
        public DateTime? CheckInTimestamp { get; set; }
        public string CheckInComentarios { get; set; }
        public decimal? CheckInLatitud { get; set; }
        public decimal? CheckInLongitud { get; set; }
        public decimal? CheckInPrecision { get; set; }
        public string CheckInDireccion { get; set; }

        // Check-out data
        public DateTime? CheckOutTimestamp { get; set; }
        public string CheckOutComentarios { get; set; }
        public decimal? CheckOutLatitud { get; set; }
        public decimal? CheckOutLongitud { get; set; }
        public decimal? CheckOutPrecision { get; set; }
        public string CheckOutDireccion { get; set; }
        public int? DuracionMinutos { get; set; }

        [Required]
        [StringLength(20)]
        public string Estatus { get; set; } // en_proceso, completada, cancelada

        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public DateTime? FechaCancelacion { get; set; }
        public string MotivoCancelacion { get; set; }

        public int? RutaId { get; set; }

        // Foreign keys
        [ForeignKey("LiderClave")]
        public virtual LiderComercialEntity Lider { get; set; }

        [ForeignKey("ClienteId")]
        public virtual NegocioEntity Cliente { get; set; }

        [ForeignKey("RutaId")]
        public virtual RutaEntity Ruta { get; set; }

        [ForeignKey("PlanId")]
        public virtual PlanTrabajoEntity Plan { get; set; }

        // TEMPORALMENTE COMENTADO - Navigation property for forms
        // public virtual VisitaFormularioEntity Formulario { get; set; }
    }
}