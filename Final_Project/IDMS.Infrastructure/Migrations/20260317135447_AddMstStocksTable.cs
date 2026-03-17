using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IDMS.Infrastructure.Migrations
{
    public partial class AddMstStocksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mst_stocks_mst_models_model_id",
                table: "mst_stocks");
            // --- BAGIAN INI DIHAPUS KARENA MENCOBA MENGUBAH TABEL MODEL YANG SUDAH ADA ---
            // (Semua perintah DropForeignKey, Rename, AddColumn, dll untuk mst_models dihapus)

            // --- BAGIAN INI TETAP ADA UNTUK MEMPERBAIKI INDEX JIKA DIPERLUKAN ---
            // Jika tabel mst_stocks sudah ada tapi indexnya salah:
            migrationBuilder.DropIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks");

            // --- BAGIAN UTAMA: MEMBUAT INDEX UNIQUE DAN FOREIGN KEY ---
            migrationBuilder.CreateIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks",
                column: "model_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_mst_stocks_mst_models",
                table: "mst_stocks",
                column: "model_id",
                principalTable: "mst_models", // Nama tabel di DB harus sesuai
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_mst_stocks_mst_models",
                table: "mst_stocks");

            migrationBuilder.DropIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks");
            
            // Tambahkan index lama jika ingin rollback
            migrationBuilder.CreateIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks",
                column: "model_id");
        }
    }
}