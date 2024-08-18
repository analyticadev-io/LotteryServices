using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicBackendTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AddModulesPropoerties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "module_name",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "visibilityStatus",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "module_name",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "visibilityStatus",
                table: "Modules");
        }
    }
}
