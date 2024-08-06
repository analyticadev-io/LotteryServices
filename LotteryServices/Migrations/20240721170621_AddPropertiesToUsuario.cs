using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryServices.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sorteos",
                columns: table => new
                {
                    SorteoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaSorteo = table.Column<DateTime>(type: "datetime", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sorteos__78DF2ECD65F4C8D8", x => x.SorteoID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__2B3DE79801DD5826", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "NumerosSorteos",
                columns: table => new
                {
                    NumeroSorteoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SorteoID = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NumerosS__C7283F19987C77E6", x => x.NumeroSorteoID);
                    table.ForeignKey(
                        name: "FK__NumerosSo__Sorte__44FF419A",
                        column: x => x.SorteoID,
                        principalTable: "Sorteos",
                        principalColumn: "SorteoID");
                });

            migrationBuilder.CreateTable(
                name: "Boletos",
                columns: table => new
                {
                    BoletoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    SorteoID = table.Column<int>(type: "int", nullable: false),
                    FechaCompra = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Boletos__C5AF44348C3A30AB", x => x.BoletoID);
                    table.ForeignKey(
                        name: "FK__Boletos__SorteoI__3F466844",
                        column: x => x.SorteoID,
                        principalTable: "Sorteos",
                        principalColumn: "SorteoID");
                    table.ForeignKey(
                        name: "FK__Boletos__Usuario__3E52440B",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "NumerosBoletos",
                columns: table => new
                {
                    NumeroBoletoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoletoID = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NumerosB__CF017B1BAA2D14AC", x => x.NumeroBoletoID);
                    table.ForeignKey(
                        name: "FK__NumerosBo__Bolet__4222D4EF",
                        column: x => x.BoletoID,
                        principalTable: "Boletos",
                        principalColumn: "BoletoID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boletos_SorteoID",
                table: "Boletos",
                column: "SorteoID");

            migrationBuilder.CreateIndex(
                name: "IX_Boletos_UsuarioID",
                table: "Boletos",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_NumerosBoletos_BoletoID",
                table: "NumerosBoletos",
                column: "BoletoID");

            migrationBuilder.CreateIndex(
                name: "IX_NumerosSorteos_SorteoID",
                table: "NumerosSorteos",
                column: "SorteoID");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__A9D10534029CD87A",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumerosBoletos");

            migrationBuilder.DropTable(
                name: "NumerosSorteos");

            migrationBuilder.DropTable(
                name: "Boletos");

            migrationBuilder.DropTable(
                name: "Sorteos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
