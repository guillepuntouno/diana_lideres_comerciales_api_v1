using System.Data.Entity;
using DIANA.Maestros.Models.Entities;

namespace DIANA.Maestros.Data
{
    public class DianaDbContext : DbContext
    {
        public DianaDbContext() : base("name=DianaConnection")
        {
            // Configure Entity Framework
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        // DbSets
        public DbSet<LiderComercialEntity> LideresComerciales { get; set; }
        public DbSet<RutaEntity> Rutas { get; set; }
        public DbSet<NegocioEntity> Negocios { get; set; }
        public DbSet<VisitaEntity> Visitas { get; set; }
        public DbSet<VisitaFormularioEntity> VisitaFormularios { get; set; }
        public DbSet<CompromisoEntity> Compromisos { get; set; }
        public DbSet<PlanTrabajoEntity> PlanesTrabajos { get; set; }
        public DbSet<PlanDiaEntity> PlanDias { get; set; }
        public DbSet<PlanDiaClienteEntity> PlanDiaClientes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure relationships
            
            // Lider -> Rutas (1:N)
            modelBuilder.Entity<LiderComercialEntity>()
                .HasMany(l => l.Rutas)
                .WithRequired(r => r.Lider)
                .HasForeignKey(r => r.LiderClave);

            // Ruta -> Negocios (1:N)
            modelBuilder.Entity<RutaEntity>()
                .HasMany(r => r.Negocios)
                .WithRequired(n => n.Ruta)
                .HasForeignKey(n => n.RutaId);

            // TEMPORALMENTE COMENTADO - Visita -> Formulario (1:0..1)
            // modelBuilder.Entity<VisitaFormularioEntity>()
            //     .HasRequired(f => f.Visita)
            //     .WithOptional(v => v.Formulario);

            // TEMPORALMENTE COMENTADO - Formulario -> Compromisos (1:N)
            // modelBuilder.Entity<VisitaFormularioEntity>()
            //     .HasMany(f => f.Compromisos)
            //     .WithRequired(c => c.Formulario)
            //     .HasForeignKey(c => c.FormularioId);

            // Plan -> Dias (1:N)
            modelBuilder.Entity<PlanTrabajoEntity>()
                .HasMany(p => p.Dias)
                .WithRequired(d => d.Plan)
                .HasForeignKey(d => d.PlanId);

            // PlanDia -> Clientes (1:N)
            modelBuilder.Entity<PlanDiaEntity>()
                .HasMany(d => d.ClientesAsignados)
                .WithRequired(c => c.PlanDia)
                .HasForeignKey(c => c.PlanDiaId);

            // Coordinates are now double type (no precision configuration needed)

            base.OnModelCreating(modelBuilder);
        }
    }
}