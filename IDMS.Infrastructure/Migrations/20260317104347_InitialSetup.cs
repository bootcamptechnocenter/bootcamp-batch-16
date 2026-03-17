using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IDMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "mst_brands",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            //         code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
            //         name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
            //         is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //         created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
            //         created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         deleted_by = table.Column<string>(type: "varchar(100)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("mst_brands_pkey", x => x.id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "mst_user",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            //         email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
            //         created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         deleted_by = table.Column<string>(type: "varchar(100)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("pk_mst_user", x => x.id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "mst_types",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            //         brand_id = table.Column<int>(type: "integer", nullable: false),
            //         code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
            //         name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
            //         is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //         created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
            //         created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         deleted_by = table.Column<string>(type: "varchar(100)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("mst_types_pkey", x => x.id);
            //         table.ForeignKey(
            //             name: "mst_types_brand_id_fkey",
            //             column: x => x.brand_id,
            //             principalTable: "mst_brands",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "mst_models",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            //         type_id = table.Column<int>(type: "integer", nullable: false),
            //         code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
            //         name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
            //         year = table.Column<int>(type: "integer", nullable: false),
            //         is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //         created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
            //         created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         deleted_by = table.Column<string>(type: "varchar(100)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("mst_models_pkey", x => x.id);
            //         table.ForeignKey(
            //             name: "mst_models_type_id_fkey",
            //             column: x => x.type_id,
            //             principalTable: "mst_types",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            migrationBuilder.CreateTable(
                name: "mst_stock",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    model_id = table.Column<int>(type: "integer", nullable: false),
                    jumlah_stock = table.Column<int>(type: "integer", nullable: false),
                    harga = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_by = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mst_stock_pkey", x => x.id);
                    table.ForeignKey(
                        name: "mst_stocks_model_id_fkey",
                        column: x => x.model_id,
                        principalTable: "mst_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            // migrationBuilder.CreateIndex(
            //     name: "IX_mst_models_type_id",
            //     table: "mst_models",
            //     column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_mst_stock_model_id",
                table: "mst_stock",
                column: "model_id");

            // migrationBuilder.CreateIndex(
            //     name: "IX_mst_types_brand_id",
            //     table: "mst_types",
            //     column: "brand_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mst_stock");

            migrationBuilder.DropTable(
                name: "mst_user");

            migrationBuilder.DropTable(
                name: "mst_models");

            migrationBuilder.DropTable(
                name: "mst_types");

            migrationBuilder.DropTable(
                name: "mst_brands");
        }
    }
}
