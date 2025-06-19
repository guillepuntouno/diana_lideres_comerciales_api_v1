using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("PlanesTrabajos")]
    public class PlanTrabajoEntity
    {
        [Key]
        [StringLength(50)]
        public string PlanId { get; set; }

        [Required]
        [StringLength(20)]
        public string LiderClave { get; set; }

        public int Semana { get; set; }
        public int Anio { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        [StringLength(20)]
        public string Estatus { get; set; } // borrador, aprobado, en_proceso, completado

        // Foreign key
        [ForeignKey("LiderClave")]
        public virtual LiderComercialEntity Lider { get; set; }

        // Navigation properties
        public virtual ICollection<PlanDiaEntity> Dias { get; set; }
        public virtual ICollection<VisitaEntity> Visitas { get; set; }
    }
}