using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIANA.Maestros.Models.Entities
{
    [Table("PlanDiaClientes")]
    public class PlanDiaClienteEntity
    {
        [Key]
        public int Id { get; set; }

        public int PlanDiaId { get; set; }

        [Required]
        [StringLength(20)]
        public string ClienteId { get; set; }

        [StringLength(100)]
        public string ClienteNombre { get; set; }

        [StringLength(200)]
        public string ClienteDireccion { get; set; }

        [StringLength(50)]
        public string ClienteTipo { get; set; }

        public bool Visitado { get; set; }
        public DateTime? FechaVisita { get; set; }

        // Foreign key
        [ForeignKey("PlanDiaId")]
        public virtual PlanDiaEntity PlanDia { get; set; }
    }
}