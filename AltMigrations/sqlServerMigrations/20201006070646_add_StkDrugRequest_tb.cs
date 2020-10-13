using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class add_StkDrugRequest_tb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StkDrugRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<string>(nullable: false),
                    StkDrugId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkDrugRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StkDrugRequests_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StkDrugRequests_StkDrugs_StkDrugId",
                        column: x => x.StkDrugId,
                        principalTable: "StkDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugRequests_PharmacyId",
                table: "StkDrugRequests",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_StkDrugRequests_StkDrugId_PharmacyId",
                table: "StkDrugRequests",
                columns: new[] { "StkDrugId", "PharmacyId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StkDrugRequests");
        }
    }
}
