using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Investigacion.DataAccess.EntityFramework
{
    public partial class InvestigacionDBContext : DbContext
    {
        public InvestigacionDBContext()
        {
        }

        public InvestigacionDBContext(DbContextOptions<InvestigacionDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TAB_ESPECIALIDAD> TAB_ESPECIALIDAD { get; set; }
        public virtual DbSet<TAB_INFORMACION_INVESTIGADOR> TAB_INFORMACION_INVESTIGADOR { get; set; }
        public virtual DbSet<TAB_INVESTIGADOR> TAB_INVESTIGADOR { get; set; }
        public virtual DbSet<TAB_ROL> TAB_ROL { get; set; }
        public virtual DbSet<TAB_TIPO_TRABAJO> TAB_TIPO_TRABAJO { get; set; }
        public virtual DbSet<TAB_TRABAJO> TAB_TRABAJO { get; set; }
        public virtual DbSet<TAB_USUARIO> TAB_USUARIO { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TAB_ESPECIALIDAD>(entity =>
            {
                entity.HasKey(e => e.LLP_Id)
                    .HasName("PK__TAB_ESPE__1D3AF561A669E4EA");

                entity.Property(e => e.Consecutivo).IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TAB_INFORMACION_INVESTIGADOR>(entity =>
            {
                entity.HasKey(e => e.LLP_Id)
                    .HasName("PK__TAB_INFO__1D3AF56188F9CB83");

                entity.Property(e => e.Consecutivo).IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pais)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.LLF_EspecialidadNavigation)
                    .WithMany(p => p.TAB_INFORMACION_INVESTIGADOR)
                    .HasForeignKey(d => d.LLF_Especialidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ESPECIALIDAD_INFORMACION_INVESTIGADOR");

                entity.HasOne(d => d.LLF_InvestigadorNavigation)
                    .WithMany(p => p.TAB_INFORMACION_INVESTIGADOR)
                    .HasForeignKey(d => d.LLF_Investigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INVESTIGADOR_INFORMACION_INVESTIGADOR");
            });

            modelBuilder.Entity<TAB_INVESTIGADOR>(entity =>
            {
                entity.HasKey(e => e.LLP_Id)
                    .HasName("PK__tmp_ms_x__1D3AF56171AF2FC9");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Consecutivo).IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TAB_ROL>(entity =>
            {
                entity.HasKey(e => e.LLP_Id)
                    .HasName("PK__TAB_ROL__1D3AF56132FEB988");

                entity.Property(e => e.Consecutivo)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TAB_TIPO_TRABAJO>(entity =>
            {
                entity.HasKey(e => e.LLP_Id)
                    .HasName("PK__tmp_ms_x__1D3AF5618D60CFD8");

                entity.Property(e => e.Consecutivo).IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TAB_TRABAJO>(entity =>
            {
                entity.HasKey(e => e.LLP_Id)
                    .HasName("PK__tmp_ms_x__1D3AF5615364AF3E");

                entity.Property(e => e.Consecutivo).IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.LLF_InvestigadorNavigation)
                    .WithMany(p => p.TAB_TRABAJO)
                    .HasForeignKey(d => d.LLF_Investigador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INVESTIGADOR_TRABAJO");

                entity.HasOne(d => d.LLF_TipoTrabajoNavigation)
                    .WithMany(p => p.TAB_TRABAJO)
                    .HasForeignKey(d => d.LLF_TipoTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INVESTIGADOR_TIPO_TRABAJO");
            });

            modelBuilder.Entity<TAB_USUARIO>(entity =>
            {
                entity.HasKey(e => e.LLP_Id)
                    .HasName("PK__TAB_USUA__1D3AF561EB66D83A");

                entity.Property(e => e.Consecutivo)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
