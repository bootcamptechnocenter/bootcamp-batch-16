using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IDMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class stockTableCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.RenameColumn(
            //     name: "DeletedBy",
            //     table: "mst_types",
            //     newName: "deleted_by");

            // migrationBuilder.AlterColumn<string>(
            //     name: "deleted_by",
            //     table: "mst_types",
            //     type: "varchar(100)",
            //     maxLength: 100,
            //     nullable: true,
            //     oldClrType: typeof(string),
            //     oldType: "text",
            //     oldNullable: true);

            // migrationBuilder.CreateTable(
            // name: "mst_models",
            // columns: table => new
            // {
            //     id = table.Column<int>(type: "integer", nullable: false)
            //         .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            //     code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
            //     name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
            //     year = table.Column<int>(type: "int", nullable: false),
            //     is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //     type_id = table.Column<int>(type: "int", nullable: false),
            //     created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //     created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //     updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //     updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //     deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //     deleted_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
            // },
            // constraints: table =>
            // {
            //     table.PrimaryKey("mst_models_pkey", x => x.id);
            //     table.ForeignKey(
            //         name: "mst_models_type_id_fkey",
            //         column: x => x.type_id,
            //         principalTable: "mst_types",
            //         principalColumn: "id",
            //         onDelete: ReferentialAction.Restrict);
            // });

            migrationBuilder.CreateTable(
                name: "mst_stocks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    total_stock = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mst_stocks_pkey", x => x.id);
                    table.ForeignKey(
                        name: "mst_stocks_model_id_fkey",
                        column: x => x.model_id,
                        principalTable: "mst_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mst_users_email",
                table: "mst_users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mst_models_type_id",
                table: "mst_models",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_mst_stocks_model_id",
                table: "mst_stocks",
                column: "model_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mst_stocks");

            migrationBuilder.DropTable(
                name: "mst_models");

            migrationBuilder.DropIndex(
                name: "IX_mst_users_email",
                table: "mst_users");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "mst_types",
                newName: "DeletedBy");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "mst_types",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
