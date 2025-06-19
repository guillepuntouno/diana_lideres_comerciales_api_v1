using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("Rutas")]
    public class RutaEntity
    {
        [Key]
        public int RutaId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Asesor { get; set; }

        [Required]
        [StringLength(20)]
        public string LiderClave { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }

        // Foreign key
        [ForeignKey("LiderClave")]
        public virtual LiderComercialEntity Lider { get; set; }

        // Navigation properties
        public virtual ICollection<NegocioEntity> Negocios { get; set; }
        public virtual ICollection<VisitaEntity> Visitas { get; set; }
    }
}