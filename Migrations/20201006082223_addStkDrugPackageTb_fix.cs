using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class addStkDrugPackageTb_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StkDrugRequests_Pharmacies_PharmacyId",
                table: "StkDrugRequests");

            migrationBuilder.DropIndex(
                name: "IX_StkDrugRequests_PharmacyId",
                table: "StkDrugRequests");

            migrationBuilder.CreateTable(
                name: "StkDrugPackagesRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: false),
                    StkDrugsIds = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugPackagesRequests_PharmacyId",
                table: "StkDrugPackagesRequests",
                column: "PharmacyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StkDrugPackagesRequests");

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugRequests_PharmacyId",
                table: "StkDrugRequests",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_StkDrugRequests_Pharmacies_PharmacyId",
                table: "StkDrugRequests",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
