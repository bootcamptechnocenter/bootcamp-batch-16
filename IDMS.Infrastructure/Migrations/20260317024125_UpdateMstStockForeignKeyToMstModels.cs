using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IDMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMstStockForeignKeyToMstModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "mst_stock_model_id_fkey",
                table: "mst_stock");

            migrationBuilder.AddForeignKey(
                name: "mst_stock_model_id_fkey",
                table: "mst_stock",
                column: "model_id",
                principalTable: "mst_models",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "mst_stock_model_id_fkey",
                table: "mst_stock");

            migrationBuilder.AddForeignKey(
                name: "mst_stock_model_id_fkey",
                table: "mst_stock",
                column: "model_id",
                principalTable: "mst_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
