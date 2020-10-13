using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class dbUpdate2 : Migration
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
                name: "StkDrugPackagesRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: false),
                    PackageDetails = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkDrugPackagesRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StkDrugPackagesRequests_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StocksWithPharmaClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StockId = table.Column<string>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockInStkDrgPackageReqs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StockId = table.Column<string>(nullable: false),
                    PackageId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInStkDrgPackageReqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockInStkDrgPackageReqs_StkDrugPackagesRequests_PackageId",
                        column: x => x.PackageId,
                        principalTable: "StkDrugPackagesRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StockInStkDrgPackageReqs_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmaciesInStockClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: true),
                    StockClassId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaciesInStockClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmaciesInStockClasses_StocksWithPharmaClasses_StockClassId",
                        column: x => x.StockClassId,
                        principalTable: "StocksWithPharmaClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StkDrugInStkDrgPackagesRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StkDrugId = table.Column<Guid>(nullable: false),
                    StkPackageId = table.Column<Guid>(nullable: false),
                    StockId = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkDrugInStkDrgPackagesRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StkDrugInStkDrgPackagesRequests_StkDrugs_StkDrugId",
                        column: x => x.StkDrugId,
                        principalTable: "StkDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StkDrugInStkDrgPackagesRequests_StockInStkDrgPackageReqs_StkPackageId",
                        column: x => x.StkPackageId,
                        principalTable: "StockInStkDrgPackageReqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStockClasses_StockClassId",
                table: "PharmaciesInStockClasses",
                column: "StockClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStockClasses_PharmacyId_StockClassId",
                table: "PharmaciesInStockClasses",
                columns: new[] { "PharmacyId", "StockClassId" },
                unique: true,
                filter: "[PharmacyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                column: "StkPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkDrugId_StkPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                columns: new[] { "StkDrugId", "StkPackageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugPackagesRequests_PharmacyId",
                table: "StkDrugPackagesRequests",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInStkDrgPackageReqs_PackageId",
                table: "StockInStkDrgPackageReqs",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInStkDrgPackageReqs_StockId_PackageId",
                table: "StockInStkDrgPackageReqs",
                columns: new[] { "StockId", "PackageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StocksWithPharmaClasses_StockId_ClassName",
                table: "StocksWithPharmaClasses",
                columns: new[] { "StockId", "ClassName" },
                unique: true,
                filter: "[ClassName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PharmaciesInStockClasses");

            migrationBuilder.DropTable(
                name: "StkDrugInStkDrgPackagesRequests");

            migrationBuilder.DropTable(
                name: "StocksWithPharmaClasses");

            migrationBuilder.DropTable(
                name: "StockInStkDrgPackageReqs");

            migrationBuilder.DropTable(
                name: "StkDrugPackagesRequests");

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
