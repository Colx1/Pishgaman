using Microsoft.EntityFrameworkCore.Migrations;

namespace Pishgaman.Migrations
{
    public partial class db4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedByUserID",
                table: "Tbl_Persons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Persons_AddedByUserID",
                table: "Tbl_Persons",
                column: "AddedByUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Persons_AspNetUsers_AddedByUserID",
                table: "Tbl_Persons",
                column: "AddedByUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Persons_AspNetUsers_AddedByUserID",
                table: "Tbl_Persons");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Persons_AddedByUserID",
                table: "Tbl_Persons");

            migrationBuilder.DropColumn(
                name: "AddedByUserID",
                table: "Tbl_Persons");
        }
    }
}
