using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IDMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStockRelationToOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks");

            migrationBuilder.CreateIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks",
                column: "model_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks");

            migrationBuilder.CreateIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks",
                column: "model_id",
                unique: true);
        }
    }
}
