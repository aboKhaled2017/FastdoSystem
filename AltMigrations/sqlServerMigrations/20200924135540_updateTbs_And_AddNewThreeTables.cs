using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class updateTbs_And_AddNewThreeTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PharmasClasses",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "PharmacyClass",
                table: "PharmaciesInStocks");

            migrationBuilder.CreateTable(
                name: "StocksWithPharmaClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StockId = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocksWithPharmaClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocksWithPharmaClasses_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PharmaciesInStockClasses",
                columns: table => new
                {
                    PharmacyId = table.Column<string>(nullable: false),
                    StockClassId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaciesInStockClasses", x => new { x.PharmacyId, x.StockClassId });
                    table.ForeignKey(
                        name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PharmaciesInStockClasses_StocksWithPharmaClasses_StockClassId",
                        column: x => x.StockClassId,
                        principalTable: "StocksWithPharmaClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStockClasses_StockClassId",
                table: "PharmaciesInStockClasses",
                column: "StockClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StocksWithPharmaClasses_StockId_ClassName",
                table: "StocksWithPharmaClasses",
                columns: new[] { "StockId", "ClassName" },
                unique: true,
                filter: "[StockId] IS NOT NULL AND [ClassName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PharmaciesInStockClasses");

            migrationBuilder.DropTable(
                name: "StocksWithPharmaClasses");

            migrationBuilder.AddColumn<string>(
                name: "PharmasClasses",
                table: "Stocks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PharmacyClass",
                table: "PharmaciesInStocks",
                nullable: true);
        }
    }
}
