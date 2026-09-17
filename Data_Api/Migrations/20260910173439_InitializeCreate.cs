using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data_Api.Migrations
{
    /// <inheritdoc />
    public partial class InitializeCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PontLogok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PontId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Valtozas = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PontLogok", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Szovegek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(type: "longtext", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Szovegek", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tanulok",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Osztaly = table.Column<string>(type: "longtext", nullable: false),
                    Pont = table.Column<int>(type: "int", nullable: false),
                    LastModify = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanulok", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FeladatHianyok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TanuloId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Osztaly = table.Column<string>(type: "longtext", nullable: false),
                    FeladatId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    Hianyzik = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeladatHianyok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeladatHianyok_Tanulok_TanuloId",
                        column: x => x.TanuloId,
                        principalTable: "Tanulok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HibasFeladatok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TanuloId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    Leiras = table.Column<string>(type: "longtext", nullable: false),
                    Osztaly = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HibasFeladatok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HibasFeladatok_Tanulok_TanuloId",
                        column: x => x.TanuloId,
                        principalTable: "Tanulok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pontok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TanuloId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PontSzam = table.Column<int>(type: "int", nullable: false),
                    Jegyzet = table.Column<string>(type: "longtext", nullable: false),
                    PontTipus = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pontok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pontok_Tanulok_TanuloId",
                        column: x => x.TanuloId,
                        principalTable: "Tanulok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Szorgalmik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TanuloId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Osztaly = table.Column<string>(type: "longtext", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Feladatok_szama = table.Column<int>(type: "int", nullable: false),
                    Pont = table.Column<int>(type: "int", nullable: false),
                    Jegyzet = table.Column<string>(type: "longtext", nullable: false),
                    FeladatTipus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Szorgalmik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Szorgalmik_Tanulok_TanuloId",
                        column: x => x.TanuloId,
                        principalTable: "Tanulok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FeladatHianyok_TanuloId",
                table: "FeladatHianyok",
                column: "TanuloId");

            migrationBuilder.CreateIndex(
                name: "IX_HibasFeladatok_TanuloId",
                table: "HibasFeladatok",
                column: "TanuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Pontok_TanuloId",
                table: "Pontok",
                column: "TanuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Szorgalmik_TanuloId",
                table: "Szorgalmik",
                column: "TanuloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeladatHianyok");

            migrationBuilder.DropTable(
                name: "HibasFeladatok");

            migrationBuilder.DropTable(
                name: "PontLogok");

            migrationBuilder.DropTable(
                name: "Pontok");

            migrationBuilder.DropTable(
                name: "Szorgalmik");

            migrationBuilder.DropTable(
                name: "Szovegek");

            migrationBuilder.DropTable(
                name: "Tanulok");
        }
    }
}
