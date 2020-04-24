using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AES_SOBS_PS.Models
{
    public partial class AesSobsDbContext : DbContext
    {
        public AesSobsDbContext()
        {
        }

        public AesSobsDbContext(DbContextOptions<AesSobsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Cotizacion> Cotizacion { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<RespuestaCotizacion> RespuestaCotizacion { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<TokenUser> TokenUser { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Vista> Vista { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
  
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreServicio)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cotizacion>(entity =>
            {
                entity.HasKey(e => e.IdCotizacion);

                entity.Property(e => e.FechaCotizacion).HasColumnType("datetime");

                entity.Property(e => e.Procesada).HasDefaultValueSql("((0))");

                entity.Property(e => e.Referencia)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(((((0)-(0))-(0))-(0))-(0.))");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Cotizacion)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cotizacion_Producto");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Cotizacion)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cotizacion_Usuario");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Imagen)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenciaProductoProveedor)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK_Producto_Categoria");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Producto_Usuario");
            });

            modelBuilder.Entity<RespuestaCotizacion>(entity =>
            {
                entity.HasKey(e => e.IdRespuestaCotizacion);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.FechaDisponibilidad).HasColumnType("datetime");

                entity.Property(e => e.FechaRespuesta).HasColumnType("datetime");

                entity.HasOne(d => d.IdCotizacionNavigation)
                    .WithMany(p => p.RespuestaCotizacion)
                    .HasForeignKey(d => d.IdCotizacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RespuestaCotizacion_Cotizacion");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.NombreRol)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TokenUser>(entity =>
            {
                entity.HasKey(e => e.IdToken);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__TokenUse__A9D105343FD1B15A")
                    .IsUnique();

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SignToken)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Usuario__A9D10534BA41D9F3")
                    .IsUnique();

                entity.Property(e => e.Contacto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Identificacion)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreSuscripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TokenMedio)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Rol");
            });

            modelBuilder.Entity<Vista>(entity =>
            {
                entity.HasKey(e => e.IdVista);

                entity.Property(e => e.Activa).HasDefaultValueSql("((0))");

                entity.Property(e => e.ComponentClass)
                    .HasColumnName("component_class")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ComponentName)
                    .HasColumnName("component_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Route)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('/Error')");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Vista)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vista_Rol");
            });
        }
    }
}
