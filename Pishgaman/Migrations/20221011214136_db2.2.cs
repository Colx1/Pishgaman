using Microsoft.EntityFrameworkCore.Migrations;

namespace Pishgaman.Migrations
{
    public partial class db22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Persons_AspNetUsers_UserId",
                table: "Tbl_Persons");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Persons_UserId",
                table: "Tbl_Persons");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tbl_Persons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tbl_Persons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Persons_UserId",
                table: "Tbl_Persons",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Persons_AspNetUsers_UserId",
                table: "Tbl_Persons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
