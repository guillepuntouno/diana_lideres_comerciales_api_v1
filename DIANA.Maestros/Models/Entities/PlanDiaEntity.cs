using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("PlanDias")]
    public class PlanDiaEntity
    {
        [Key]
        public int PlanDiaId { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanId { get; set; }

        [Required]
        [StringLength(20)]
        public string Dia { get; set; } // Lunes, Martes, etc.

        [Required]
        [StringLength(100)]
        public string Objetivo { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } // gestion_cliente, administrativo

        [StringLength(100)]
        public string CentroDistribucion { get; set; }

        [StringLength(50)]
        public string RutaId { get; set; }

        [StringLength(50)]
        public string RutaNombre { get; set; }

        [StringLength(100)]
        public string TipoActividad { get; set; } // Para actividades administrativas

        public string Comentario { get; set; }

        public bool Completado { get; set; }

        // Foreign key
        [ForeignKey("PlanId")]
        public virtual PlanTrabajoEntity Plan { get; set; }

        // Navigation property
        public virtual ICollection<PlanDiaClienteEntity> ClientesAsignados { get; set; }
    }
}