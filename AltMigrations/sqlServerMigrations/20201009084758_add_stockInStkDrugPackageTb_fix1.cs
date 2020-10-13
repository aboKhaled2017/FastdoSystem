using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class add_stockInStkDrugPackageTb_fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockInStkDrgPackageReqs_StkDrugPackagesRequests_PackageId",
                table: "StockInStkDrgPackageReqs");
            migrationBuilder.AddForeignKey(
                name: "FK_StockInStkDrgPackageReqs_StkDrugPackagesRequests_PackageId",
                table: "StockInStkDrgPackageReqs",
                column: "PackageId",
                principalTable: "StkDrugPackagesRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
