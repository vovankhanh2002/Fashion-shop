using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanDoWeb.Access.Migrations
{
    public partial class AddProductOriginalPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OriginalPrice",
                table: "Products",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Products");
        }
    }
}
