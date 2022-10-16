using Microsoft.EntityFrameworkCore.Migrations;

namespace Pishgaman.Migrations
{
    public partial class db21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_AspNetUsers_UserId",
                table: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Tbl_Persons");

            migrationBuilder.RenameIndex(
                name: "IX_Person_UserId",
                table: "Tbl_Persons",
                newName: "IX_Tbl_Persons_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_Persons",
                table: "Tbl_Persons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Persons_AspNetUsers_UserId",
                table: "Tbl_Persons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Persons_AspNetUsers_UserId",
                table: "Tbl_Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_Persons",
                table: "Tbl_Persons");

            migrationBuilder.RenameTable(
                name: "Tbl_Persons",
                newName: "Person");

            migrationBuilder.RenameIndex(
                name: "IX_Tbl_Persons_UserId",
                table: "Person",
                newName: "IX_Person_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_AspNetUsers_UserId",
                table: "Person",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
