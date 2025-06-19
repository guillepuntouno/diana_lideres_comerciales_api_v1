using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("Compromisos")]
    public class CompromisoEntity
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; }

        [Required]
        [StringLength(200)]
        public string Detalle { get; set; }

        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; }

        [StringLength(20)]
        public string ClienteId { get; set; }

        [StringLength(100)]
        public string RutaId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // PENDIENTE, COMPLETADO, CANCELADO

        public DateTime CreatedAt { get; set; }

        public int FormularioId { get; set; }

        // TEMPORALMENTE COMENTADO - Foreign key
        // [ForeignKey("FormularioId")]
        // public virtual VisitaFormularioEntity Formulario { get; set; }
    }
}