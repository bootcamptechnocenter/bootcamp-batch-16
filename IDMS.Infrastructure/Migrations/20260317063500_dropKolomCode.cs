using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IDMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dropKolomCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "mst_stock");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "mst_stock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "mst_stock",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "mst_stock",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
