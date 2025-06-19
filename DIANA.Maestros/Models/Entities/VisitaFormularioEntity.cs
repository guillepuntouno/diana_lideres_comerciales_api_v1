using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("VisitaFormularios")]
    public class VisitaFormularioEntity
    {
        [Key]
        public int FormularioId { get; set; }

        [Required]
        [StringLength(100)]
        public string VisitaId { get; set; }

        // Sección 1 - Exhibidor
        public bool PoseeExhibidorAdecuado { get; set; }
        public string TipoExhibidorSeleccionado { get; set; }
        public string ModeloExhibidorSeleccionado { get; set; }
        public int CantidadExhibidores { get; set; }

        // Sección 2 - Posicionamiento
        public bool PrimeraPosition { get; set; }
        public bool Planograma { get; set; }
        public bool PortafolioFoco { get; set; }
        public bool Anclaje { get; set; }

        // Sección 3 - Categorías
        public bool Ristras { get; set; }
        public bool Max { get; set; }
        public bool Familiar { get; set; }
        public bool Dulce { get; set; }
        public bool Galleta { get; set; }

        // Sección 5 - Retroalimentación
        public string Retroalimentacion { get; set; }
        public string Reconocimiento { get; set; }

        public DateTime? FechaActualizacion { get; set; }
        [StringLength(10)]
        public string Version { get; set; }

        // TEMPORALMENTE COMENTADO - Foreign key
        // [ForeignKey("VisitaId")]
        // public virtual VisitaEntity Visita { get; set; }

        // TEMPORALMENTE COMENTADO - Navigation property for compromisos
        // public virtual ICollection<CompromisoEntity> Compromisos { get; set; }
    }
}