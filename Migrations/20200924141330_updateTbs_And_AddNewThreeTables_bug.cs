using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class updateTbs_And_AddNewThreeTables_bug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocksWithPharmaClasses_Stocks_StockId",
                table: "StocksWithPharmaClasses");

            migrationBuilder.AddForeignKey(
                name: "FK_StocksWithPharmaClasses_Stocks_StockId",
                table: "StocksWithPharmaClasses",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocksWithPharmaClasses_Stocks_StockId",
                table: "StocksWithPharmaClasses");

            migrationBuilder.AddForeignKey(
                name: "FK_StocksWithPharmaClasses_Stocks_StockId",
                table: "StocksWithPharmaClasses",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
