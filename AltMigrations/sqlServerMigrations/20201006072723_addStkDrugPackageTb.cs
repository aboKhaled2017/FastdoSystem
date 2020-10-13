using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class addStkDrugPackageTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StkDrugRequests_StkDrugs_StkDrugId",
                table: "StkDrugRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_StkDrugRequests_StkDrugs_StkDrugId",
                table: "StkDrugRequests",
                column: "StkDrugId",
                principalTable: "StkDrugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
