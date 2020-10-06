using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class updateStkDrugTb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                table: "PharmaciesInStockClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PharmaciesInStockClasses",
                table: "PharmaciesInStockClasses");

            migrationBuilder.AlterColumn<string>(
                name: "PharmacyId",
                table: "PharmaciesInStockClasses",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PharmaciesInStockClasses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PharmaciesInStockClasses",
                table: "PharmaciesInStockClasses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaciesInStockClasses_PharmacyId_StockClassId",
                table: "PharmaciesInStockClasses",
                columns: new[] { "PharmacyId", "StockClassId" },
                unique: true,
                filter: "[PharmacyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                table: "PharmaciesInStockClasses",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                table: "PharmaciesInStockClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PharmaciesInStockClasses",
                table: "PharmaciesInStockClasses");

            migrationBuilder.DropIndex(
                name: "IX_PharmaciesInStockClasses_PharmacyId_StockClassId",
                table: "PharmaciesInStockClasses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PharmaciesInStockClasses");

            migrationBuilder.AlterColumn<string>(
                name: "PharmacyId",
                table: "PharmaciesInStockClasses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PharmaciesInStockClasses",
                table: "PharmaciesInStockClasses",
                columns: new[] { "PharmacyId", "StockClassId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PharmaciesInStockClasses_Pharmacies_PharmacyId",
                table: "PharmaciesInStockClasses",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
