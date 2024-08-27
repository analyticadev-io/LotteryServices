using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryServices.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleToSorteo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Sorteos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Sorteos");
        }
    }
}
