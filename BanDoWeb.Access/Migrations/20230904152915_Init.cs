using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanDoWeb.Access.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Navbars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleNavBar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlNavBar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateSetNavbar = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOutNavbar = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navbars", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Navbars");
        }
    }
}
