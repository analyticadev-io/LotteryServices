﻿// <auto-generated />
using System;
using LotteryServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LotteryServices.Migrations
{
    [DbContext(typeof(LoteriaDbContext))]
    partial class LoteriaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LotteryServices.Models.Boleto", b =>
                {
                    b.Property<int>("BoletoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BoletoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BoletoId"));

                    b.Property<DateTime?>("FechaCompra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("SorteoId")
                        .HasColumnType("int")
                        .HasColumnName("SorteoID");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("UsuarioID");

                    b.HasKey("BoletoId")
                        .HasName("PK__Boletos__C5AF44348C3A30AB");

                    b.HasIndex(new[] { "SorteoId" }, "IX_Boletos_SorteoID");

                    b.HasIndex(new[] { "UsuarioId" }, "IX_Boletos_UsuarioID");

                    b.ToTable("Boletos");
                });

            modelBuilder.Entity("LotteryServices.Models.Module", b =>
                {
                    b.Property<int>("IdModule")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdModule"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("icon")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("module_name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("visibilityStatus")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdModule")
                        .HasName("PK__Permiso__96E0C723B9F1774B");

                    b.ToTable("Modules", (string)null);
                });

            modelBuilder.Entity("LotteryServices.Models.NumerosBoleto", b =>
                {
                    b.Property<int>("NumeroBoletoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("NumeroBoletoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NumeroBoletoId"));

                    b.Property<int>("BoletoId")
                        .HasColumnType("int")
                        .HasColumnName("BoletoID");

                    b.Property<long>("Numero")
                        .HasColumnType("bigint");

                    b.HasKey("NumeroBoletoId")
                        .HasName("PK__NumerosB__CF017B1BAA2D14AC");

                    b.HasIndex(new[] { "BoletoId" }, "IX_NumerosBoletos_BoletoID");

                    b.ToTable("NumerosBoletos");
                });

            modelBuilder.Entity("LotteryServices.Models.NumerosSorteo", b =>
                {
                    b.Property<int>("NumeroSorteoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("NumeroSorteoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NumeroSorteoId"));

                    b.Property<long>("Numero")
                        .HasColumnType("bigint");

                    b.Property<int>("SorteoId")
                        .HasColumnType("int")
                        .HasColumnName("SorteoID");

                    b.HasKey("NumeroSorteoId")
                        .HasName("PK__NumerosS__C7283F19987C77E6");

                    b.HasIndex(new[] { "SorteoId" }, "IX_NumerosSorteos_SorteoID");

                    b.ToTable("NumerosSorteos");
                });

            modelBuilder.Entity("LotteryServices.Models.Permiso", b =>
                {
                    b.Property<int>("PermisoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermisoId"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("PermisoId")
                        .HasName("PK__Permiso__96E0C723B9F1774B");

                    b.ToTable("Permiso", (string)null);
                });

            modelBuilder.Entity("LotteryServices.Models.Rol", b =>
                {
                    b.Property<int>("RolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RolId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("RolId")
                        .HasName("PK__Rol__F92302F1BC274A1A");

                    b.ToTable("Rol", (string)null);
                });

            modelBuilder.Entity("LotteryServices.Models.Sorteo", b =>
                {
                    b.Property<int>("SorteoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("SorteoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SorteoId"));

                    b.Property<string>("Descripcion")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("FechaSorteo")
                        .HasColumnType("datetime");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SorteoId")
                        .HasName("PK__Sorteos__78DF2ECD65F4C8D8");

                    b.ToTable("Sorteos");
                });

            modelBuilder.Entity("LotteryServices.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UsuarioID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioId"));

                    b.Property<string>("Contrasena")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsuarioId")
                        .HasName("PK__Usuarios__2B3DE79801DD5826");

                    b.HasIndex(new[] { "Email" }, "UQ__Usuarios__A9D10534029CD87A")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("RolPermiso", b =>
                {
                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.Property<int>("PermisoId")
                        .HasColumnType("int");

                    b.HasKey("RolId", "PermisoId")
                        .HasName("PK__RolPermi__D04D0E83B555BC8B");

                    b.HasIndex("PermisoId");

                    b.ToTable("RolPermiso", (string)null);
                });

            modelBuilder.Entity("UsuarioRol", b =>
                {
                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.HasKey("UsuarioId", "RolId")
                        .HasName("PK__UsuarioR__24AFD797599EC1E0");

                    b.HasIndex("RolId");

                    b.ToTable("UsuarioRol", (string)null);
                });

            modelBuilder.Entity("LotteryServices.Models.Boleto", b =>
                {
                    b.HasOne("LotteryServices.Models.Sorteo", "Sorteo")
                        .WithMany("Boletos")
                        .HasForeignKey("SorteoId")
                        .IsRequired()
                        .HasConstraintName("FK__Boletos__SorteoI__3F466844");

                    b.HasOne("LotteryServices.Models.Usuario", "Usuario")
                        .WithMany("Boletos")
                        .HasForeignKey("UsuarioId")
                        .IsRequired()
                        .HasConstraintName("FK__Boletos__Usuario__3E52440B");

                    b.Navigation("Sorteo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("LotteryServices.Models.NumerosBoleto", b =>
                {
                    b.HasOne("LotteryServices.Models.Boleto", "Boleto")
                        .WithMany("NumerosBoletos")
                        .HasForeignKey("BoletoId")
                        .IsRequired()
                        .HasConstraintName("FK__NumerosBo__Bolet__4222D4EF");

                    b.Navigation("Boleto");
                });

            modelBuilder.Entity("LotteryServices.Models.NumerosSorteo", b =>
                {
                    b.HasOne("LotteryServices.Models.Sorteo", "Sorteo")
                        .WithMany("NumerosSorteos")
                        .HasForeignKey("SorteoId")
                        .IsRequired()
                        .HasConstraintName("FK__NumerosSo__Sorte__44FF419A");

                    b.Navigation("Sorteo");
                });

            modelBuilder.Entity("RolPermiso", b =>
                {
                    b.HasOne("LotteryServices.Models.Permiso", null)
                        .WithMany()
                        .HasForeignKey("PermisoId")
                        .IsRequired()
                        .HasConstraintName("FK__RolPermis__Permi__2739D489");

                    b.HasOne("LotteryServices.Models.Rol", null)
                        .WithMany()
                        .HasForeignKey("RolId")
                        .IsRequired()
                        .HasConstraintName("FK__RolPermis__RolId__2645B050");
                });

            modelBuilder.Entity("UsuarioRol", b =>
                {
                    b.HasOne("LotteryServices.Models.Rol", null)
                        .WithMany()
                        .HasForeignKey("RolId")
                        .IsRequired()
                        .HasConstraintName("FK__UsuarioRo__RolId__2B0A656D");

                    b.HasOne("LotteryServices.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .IsRequired()
                        .HasConstraintName("FK__UsuarioRo__Usuar__2A164134");
                });

            modelBuilder.Entity("LotteryServices.Models.Boleto", b =>
                {
                    b.Navigation("NumerosBoletos");
                });

            modelBuilder.Entity("LotteryServices.Models.Sorteo", b =>
                {
                    b.Navigation("Boletos");

                    b.Navigation("NumerosSorteos");
                });

            modelBuilder.Entity("LotteryServices.Models.Usuario", b =>
                {
                    b.Navigation("Boletos");
                });
#pragma warning restore 612, 618
        }
    }
}
