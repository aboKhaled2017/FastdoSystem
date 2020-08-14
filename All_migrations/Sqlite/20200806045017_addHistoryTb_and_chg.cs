using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace System_Back_End.Migrations
{
    public partial class addHistoryTb_and_chg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Admins_SuperAdminId",
                table: "Admins");

            migrationBuilder.CreateTable(
                name: "AdminHistoryEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OccuredAt = table.Column<DateTime>(nullable: false),
                    IssuerId = table.Column<string>(nullable: true),
                    OperationType = table.Column<string>(nullable: true),
                    ToId = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminHistoryEntries", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Admins_SuperAdminId",
                table: "Admins",
                column: "SuperAdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Admins_SuperAdminId",
                table: "Admins");

            migrationBuilder.DropTable(
                name: "AdminHistoryEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Admins_SuperAdminId",
                table: "Admins",
                column: "SuperAdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
