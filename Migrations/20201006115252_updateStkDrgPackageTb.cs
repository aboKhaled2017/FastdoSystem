using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class updateStkDrgPackageTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StkDrugsIds",
                table: "StkDrugPackagesRequests",
                newName: "PackageDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "StkDrugPackagesRequests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "StkDrugPackagesRequests");

            migrationBuilder.RenameColumn(
                name: "PackageDetails",
                table: "StkDrugPackagesRequests",
                newName: "StkDrugsIds");
        }
    }
}
