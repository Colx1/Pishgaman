using Microsoft.EntityFrameworkCore.Migrations;

namespace Pishgaman.Migrations
{
    public partial class db32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Tbl_Persons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Tbl_Persons");
        }
    }
}
