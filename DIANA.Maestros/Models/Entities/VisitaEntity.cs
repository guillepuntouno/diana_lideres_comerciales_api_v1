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
        public string CheckInTimestamp { get; set; }
        public string CheckInComentarios { get; set; }
        public double? CheckInLatitud { get; set; }
        public double? CheckInLongitud { get; set; }
        public double? CheckInPrecision { get; set; }
        public string CheckInDireccion { get; set; }

        // Check-out data
        public string CheckOutTimestamp { get; set; }
        public string CheckOutComentarios { get; set; }
        public double? CheckOutLatitud { get; set; }
        public double? CheckOutLongitud { get; set; }
        public double? CheckOutPrecision { get; set; }
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

        // Navigation property for forms
        public virtual VisitaFormularioEntity Formulario { get; set; }
    }
}