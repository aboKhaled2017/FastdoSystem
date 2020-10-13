using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.backendsys.Migrations
{
    public partial class updateTb_StkDrugPackReq_AddNameProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StkDrugPackagesRequests",
                nullable: false, 
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "StkDrugPackagesRequests");
        }
    }
}
