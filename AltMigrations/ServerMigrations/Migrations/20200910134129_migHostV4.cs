using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class migHostV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StkDrugs");

 

            migrationBuilder.AddColumn<string>(
                name: "PharmasClasses",
                table: "Stocks",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StkDrugs",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Discount",
                table: "StkDrugs",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "StkDrugs",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "PharmacyClass",
                table: "PharmaciesInStocks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "PharmaciesInStocks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "PharmasClasses",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "StkDrugs");

            migrationBuilder.DropColumn(
                name: "Seen",
                table: "PharmaciesInStocks");

           

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StkDrugs",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "StkDrugs",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "StkDrugs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PharmacyClass",
                table: "PharmaciesInStocks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

        }
    }
}
