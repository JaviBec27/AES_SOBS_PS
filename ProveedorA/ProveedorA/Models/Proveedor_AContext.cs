using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProveedorA.Models
{
    public partial class Proveedor_AContext : DbContext
    {
        public Proveedor_AContext()
        {
        }

        public Proveedor_AContext(DbContextOptions<Proveedor_AContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cotizacion> Cotizacion { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<RespuestaCotizacion> RespuestaCotizacion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=CTSCO5CG9263GHD;Initial Catalog=Proveedor_A;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cotizacion>(entity =>
            {
                entity.HasKey(e => e.IdCotizacion);

                entity.Property(e => e.FechaCotizacion).HasColumnType("datetime");

                entity.Property(e => e.Referencia)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Productos>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.NombreDescripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Referencia)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RespuestaCotizacion>(entity =>
            {
                entity.HasKey(e => e.IdRespuestaCotizacion);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.FechaDisponibilidad).HasColumnType("datetime");

                entity.Property(e => e.FechaRespuesta).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
