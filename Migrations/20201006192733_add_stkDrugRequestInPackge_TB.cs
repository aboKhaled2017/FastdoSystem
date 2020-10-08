using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class add_stkDrugRequestInPackge_TB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StkDrugRequests");

            migrationBuilder.CreateTable(
                name: "StkDrugInStkDrgPackagesRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StkDrugId = table.Column<Guid>(nullable: false),
                    StkDrugPackageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkDrugInStkDrgPackagesRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StkDrugInStkDrgPackagesRequests_StkDrugs_StkDrugId",
                        column: x => x.StkDrugId,
                        principalTable: "StkDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StkDrugInStkDrgPackagesRequests_StkDrugPackagesRequests_StkDrugPackageId",
                        column: x => x.StkDrugPackageId,
                        principalTable: "StkDrugPackagesRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                column: "StkDrugPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugInStkDrgPackagesRequests_StkDrugId_StkDrugPackageId",
                table: "StkDrugInStkDrgPackagesRequests",
                columns: new[] { "StkDrugId", "StkDrugPackageId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StkDrugInStkDrgPackagesRequests");

            migrationBuilder.CreateTable(
                name: "StkDrugRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StkDrugId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkDrugRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugRequests_StkDrugId_PharmacyId",
                table: "StkDrugRequests",
                columns: new[] { "StkDrugId", "PharmacyId" },
                unique: true);
        }
    }
}
