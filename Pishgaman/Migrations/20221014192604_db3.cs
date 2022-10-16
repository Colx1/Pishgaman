using Microsoft.EntityFrameworkCore.Migrations;

namespace Pishgaman.Migrations
{
    public partial class db3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_IpAddressBlacklists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IpAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_IpAddressBlacklists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_JwtStoredTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Blocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_IpAddressBlacklists");

            migrationBuilder.DropTable(
                name: "Tbl_JwtStoredTokens");
        }
    }
}
