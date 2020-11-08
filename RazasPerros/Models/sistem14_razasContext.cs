using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RazasPerros.Models
{
    public partial class sistem14_razasContext : DbContext
    {
        public sistem14_razasContext()
        {
        }

        public sistem14_razasContext(DbContextOptions<sistem14_razasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Caracteristicasfisicas> Caracteristicasfisicas { get; set; }
        public virtual DbSet<Estadisticasraza> Estadisticasraza { get; set; }
        public virtual DbSet<Paises> Paises { get; set; }
        public virtual DbSet<Razas> Razas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=204.93.167.23; user=sistem14_u3; password=unidad_3; database=sistem14_razas");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Caracteristicasfisicas>(entity =>
            {
                entity.ToTable("caracteristicasfisicas");

                entity.Property(e => e.Cola).HasColumnType("varchar(500)");

                entity.Property(e => e.Color).HasColumnType("varchar(500)");

                entity.Property(e => e.Hocico).HasColumnType("varchar(500)");

                entity.Property(e => e.Patas).HasColumnType("varchar(500)");

                entity.Property(e => e.Pelo).HasColumnType("varchar(500)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Caracteristicasfisicas)
                    .HasForeignKey<Caracteristicasfisicas>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_caracteristicasfisicas_1");
            });

            modelBuilder.Entity<Estadisticasraza>(entity =>
            {
                entity.ToTable("estadisticasraza");

                entity.Property(e => e.Id).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Estadisticasraza)
                    .HasForeignKey<Estadisticasraza>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_estadisticasraza_1");
            });

            modelBuilder.Entity<Paises>(entity =>
            {
                entity.ToTable("paises");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Nombre).HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<Razas>(entity =>
            {
                entity.ToTable("razas");

                entity.HasIndex(e => e.IdPais)
                    .HasName("pi_idx");

                entity.Property(e => e.Id).HasDefaultValueSql("'0'");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Eliminado).HasColumnType("bit(1)");

                entity.Property(e => e.IdPais).HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.OtrosNombres).HasColumnType("varchar(500)");

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Razas)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkpai");
            });
        }
    }
}
