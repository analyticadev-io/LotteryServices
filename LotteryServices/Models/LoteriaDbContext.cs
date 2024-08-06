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

    public virtual DbSet<Sorteo> Sorteos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-TLQ4DNO;Database=LoteriaDB;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Boleto>(entity =>
        {
            entity.HasKey(e => e.BoletoId).HasName("PK__Boletos__C5AF44348C3A30AB");

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

            entity.Property(e => e.NumeroSorteoId).HasColumnName("NumeroSorteoID");
            entity.Property(e => e.SorteoId).HasColumnName("SorteoID");

            entity.HasOne(d => d.Sorteo).WithMany(p => p.NumerosSorteos)
                .HasForeignKey(d => d.SorteoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NumerosSo__Sorte__44FF419A");
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
