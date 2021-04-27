using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fastdo.API.Migrations
{
    public partial class addBaseDrug_and_TechnicalSupportTb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseDrugs",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Desc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseDrugs", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalSupportQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<string>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    Question = table.Column<string>(nullable: false),
                    Response = table.Column<string>(nullable: true),
                    SeenAt = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSupportQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalSupportQuestions_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSupportQuestions_SenderId",
                table: "TechnicalSupportQuestions",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseDrugs");

            migrationBuilder.DropTable(
                name: "TechnicalSupportQuestions");
        }
    }
}
