using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Models;

public partial class LoteriaDbContext : DbContext
{
    public LoteriaDbContext()
    {
    }

    public LoteriaDbContext(DbContextOptions<LoteriaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Boleto> Boletos { get; set; }

    public virtual DbSet<NumerosBoleto> NumerosBoletos { get; set; }

    public virtual DbSet<NumerosSorteo> NumerosSorteos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Sorteo> Sorteos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Module> Modules { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Boleto>(entity =>
        {
            entity.HasKey(e => e.BoletoId).HasName("PK__Boletos__C5AF44348C3A30AB");

            entity.HasIndex(e => e.SorteoId, "IX_Boletos_SorteoID");

            entity.HasIndex(e => e.UsuarioId, "IX_Boletos_UsuarioID");

            entity.Property(e => e.BoletoId).HasColumnName("BoletoID");
            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SorteoId).HasColumnName("SorteoID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Sorteo).WithMany(p => p.Boletos)
                .HasForeignKey(d => d.SorteoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Boletos__SorteoI__3F466844");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Boletos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Boletos__Usuario__3E52440B");
        });

        modelBuilder.Entity<NumerosBoleto>(entity =>
        {
            entity.HasKey(e => e.NumeroBoletoId).HasName("PK__NumerosB__CF017B1BAA2D14AC");

            entity.HasIndex(e => e.BoletoId, "IX_NumerosBoletos_BoletoID");

            entity.Property(e => e.NumeroBoletoId).HasColumnName("NumeroBoletoID");
            entity.Property(e => e.BoletoId).HasColumnName("BoletoID");

            entity.HasOne(d => d.Boleto).WithMany(p => p.NumerosBoletos)
                .HasForeignKey(d => d.BoletoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NumerosBo__Bolet__4222D4EF");
        });

        modelBuilder.Entity<NumerosSorteo>(entity =>
        {
            entity.HasKey(e => e.NumeroSorteoId).HasName("PK__NumerosS__C7283F19987C77E6");

            entity.HasIndex(e => e.SorteoId, "IX_NumerosSorteos_SorteoID");

            entity.Property(e => e.NumeroSorteoId).HasColumnName("NumeroSorteoID");
            entity.Property(e => e.SorteoId).HasColumnName("SorteoID");

            entity.HasOne(d => d.Sorteo).WithMany(p => p.NumerosSorteos)
                .HasForeignKey(d => d.SorteoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NumerosSo__Sorte__44FF419A");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.PermisoId).HasName("PK__Permiso__96E0C723B9F1774B");

            entity.ToTable("Permiso");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__F92302F1BC274A1A");

            entity.ToTable("Rol");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasMany(d => d.Permisos).WithMany(p => p.Rols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolPermis__Permi__2739D489"),
                    l => l.HasOne<Rol>().WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolPermis__RolId__2645B050"),
                    j =>
                    {
                        j.HasKey("RolId", "PermisoId").HasName("PK__RolPermi__D04D0E83B555BC8B");
                        j.ToTable("RolPermiso");
                    });
        });

        modelBuilder.Entity<Sorteo>(entity =>
        {
            entity.HasKey(e => e.SorteoId).HasName("PK__Sorteos__78DF2ECD65F4C8D8");

            entity.Property(e => e.SorteoId).HasColumnName("SorteoID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.FechaSorteo).HasColumnType("datetime");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE79801DD5826");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534029CD87A").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);

            //POSTGresSQL
            //entity.ToTable("Usuarios", "public");

            entity.HasMany(d => d.Rols).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioRol",
                    r => r.HasOne<Rol>().WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsuarioRo__RolId__2B0A656D"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsuarioRo__Usuar__2A164134"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "RolId").HasName("PK__UsuarioR__24AFD797599EC1E0");
                        j.ToTable("UsuarioRol");
                    });
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.IdModule).HasName("PK__Permiso__96E0C723B9F1774B");

            entity.ToTable("Modules");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.icon)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.module_name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.visibilityStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
