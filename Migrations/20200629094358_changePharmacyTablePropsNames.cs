using Microsoft.EntityFrameworkCore.Migrations;

namespace System_Back_End.Migrations
{
    public partial class changePharmacyTablePropsNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhrLicenseImgSrc",
                table: "Pharmacies",
                newName: "LicenseImgSrc");

            migrationBuilder.RenameColumn(
                name: "PhrCommercialRegImgSrc",
                table: "Pharmacies",
                newName: "CommercialRegImgSrc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LicenseImgSrc",
                table: "Pharmacies",
                newName: "PhrLicenseImgSrc");

            migrationBuilder.RenameColumn(
                name: "CommercialRegImgSrc",
                table: "Pharmacies",
                newName: "PhrCommercialRegImgSrc");
        }
    }
}
