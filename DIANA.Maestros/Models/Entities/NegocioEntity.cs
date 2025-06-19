using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("Negocios")]
    public class NegocioEntity
    {
        [Key]
        [StringLength(20)]
        public string Clave { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Canal { get; set; }

        [Required]
        [StringLength(10)]
        public string Clasificacion { get; set; }

        [StringLength(200)]
        public string Exhibidor { get; set; }

        public int RutaId { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }

        // Foreign key
        [ForeignKey("RutaId")]
        public virtual RutaEntity Ruta { get; set; }

        // Navigation properties
        public virtual ICollection<VisitaEntity> Visitas { get; set; }
    }
}