using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanDoWeb.Access.Migrations
{
    public partial class AddImgToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categoriess",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Categoriess");
        }
    }
}
