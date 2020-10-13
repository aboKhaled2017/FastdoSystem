using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class mighostv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Admins_SuperAdminId",
                table: "Admins",
                column: "SuperAdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.CreateIndex(
                name: "IX_Admins_SuperAdminId",
                table: "Admins",
                column: "SuperAdminId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Admins_SuperAdminId",
                table: "Admins");

        }
    }
}
