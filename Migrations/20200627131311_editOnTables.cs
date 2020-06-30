using Microsoft.EntityFrameworkCore.Migrations;

namespace System_Back_End.Migrations
{
    public partial class editOnTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StPhone",
                table: "Stocks",
                newName: "LandlinePhone");

            migrationBuilder.RenameColumn(
                name: "PhrPhone",
                table: "Pharmacies",
                newName: "LandlinePhone");

            migrationBuilder.AddColumn<string>(
                name: "confirmCode",
                table: "AspNetUsers",
                maxLength: 8,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmCode",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LandlinePhone",
                table: "Stocks",
                newName: "StPhone");

            migrationBuilder.RenameColumn(
                name: "LandlinePhone",
                table: "Pharmacies",
                newName: "PhrPhone");
        }
    }
}
