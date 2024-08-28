using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryServices.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyToNumeroOfNumeroSorteo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Numero",
                table: "NumerosSorteos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                table: "NumerosSorteos",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
