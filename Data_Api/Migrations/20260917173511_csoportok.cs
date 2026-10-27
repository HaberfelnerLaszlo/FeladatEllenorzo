using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data_Api.Migrations
{
    /// <inheritdoc />
    public partial class csoportok : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CsoportId",
                table: "Pontok",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Beiratkozasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TanuloId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CsoportId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beiratkozasok", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Csoportok",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Csoportok", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CsoportTanulo",
                columns: table => new
                {
                    CsoportokId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TanulokId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsoportTanulo", x => new { x.CsoportokId, x.TanulokId });
                    table.ForeignKey(
                        name: "FK_CsoportTanulo_Csoportok_CsoportokId",
                        column: x => x.CsoportokId,
                        principalTable: "Csoportok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CsoportTanulo_Tanulok_TanulokId",
                        column: x => x.TanulokId,
                        principalTable: "Tanulok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CsoportTanulo_TanulokId",
                table: "CsoportTanulo",
                column: "TanulokId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beiratkozasok");

            migrationBuilder.DropTable(
                name: "CsoportTanulo");

            migrationBuilder.DropTable(
                name: "Csoportok");

            migrationBuilder.DropColumn(
                name: "CsoportId",
                table: "Pontok");
        }
    }
}
