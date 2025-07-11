using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("LideresComerciales")]
    public class LiderComercialEntity
    {
        [Key]
        [StringLength(20)]
        public string Clave { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Pais { get; set; }

        [Required]
        [StringLength(100)]
        public string CentroDistribucion { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }

        // Navigation properties
        public virtual ICollection<RutaEntity> Rutas { get; set; }
        public virtual ICollection<VisitaEntity> Visitas { get; set; }
        public virtual ICollection<PlanTrabajoEntity> Planes { get; set; }
    }
}