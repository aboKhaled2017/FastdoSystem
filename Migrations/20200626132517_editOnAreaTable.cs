using Microsoft.EntityFrameworkCore.Migrations;

namespace System_Back_End.Migrations
{
    public partial class editOnAreaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Areas_AreaId",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_AreaId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "SuperArea",
                table: "Areas",
                newName: "SuperAreaId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_SuperAreaId",
                table: "Areas",
                column: "SuperAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Areas_SuperAreaId",
                table: "Areas",
                column: "SuperAreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Areas_SuperAreaId",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_SuperAreaId",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "SuperAreaId",
                table: "Areas",
                newName: "SuperArea");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<byte>(
                name: "AreaId",
                table: "Areas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_AreaId",
                table: "Areas",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Areas_AreaId",
                table: "Areas",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
