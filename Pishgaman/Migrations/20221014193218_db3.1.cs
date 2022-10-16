using Microsoft.EntityFrameworkCore.Migrations;

namespace Pishgaman.Migrations
{
    public partial class db31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tbl_JwtStoredTokens",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Tbl_JwtStoredTokens",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_JwtStoredTokens",
                table: "Tbl_JwtStoredTokens",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_JwtStoredTokens_UserId",
                table: "Tbl_JwtStoredTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_JwtStoredTokens_AspNetUsers_UserId",
                table: "Tbl_JwtStoredTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_JwtStoredTokens_AspNetUsers_UserId",
                table: "Tbl_JwtStoredTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_JwtStoredTokens",
                table: "Tbl_JwtStoredTokens");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_JwtStoredTokens_UserId",
                table: "Tbl_JwtStoredTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Tbl_JwtStoredTokens");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tbl_JwtStoredTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
