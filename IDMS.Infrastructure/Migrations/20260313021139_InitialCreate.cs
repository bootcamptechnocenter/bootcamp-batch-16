using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IDMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
            //         created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         deleted_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("mst_brands_pkey", x => x.id);
            //     });

            migrationBuilder.CreateTable(
                name: "mst_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mst_users_pkey", x => x.id);
                });

            // migrationBuilder.CreateTable(
            //     name: "mst_types",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
            //         code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
            //         name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
            //         is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //         brand_id = table.Column<int>(type: "int", nullable: false),
            //         created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            //         created_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         updated_by = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
            //         DeletedBy = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("mst_types_pkey", x => x.id);
            //         table.ForeignKey(
            //             name: "mst_types_brand_id_fkey",
            //             column: x => x.brand_id,
            //             principalTable: "mst_brands",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_mst_types_brand_id",
                table: "mst_types",
                column: "brand_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "mst_types");

            migrationBuilder.DropTable(
                name: "mst_users");

            // migrationBuilder.DropTable(
            //     name: "mst_brands");
        }
    }
}
