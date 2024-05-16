using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanDoWeb.Access.Migrations
{
    public partial class AddNumberOfVisitsApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "NumberOfVisits",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NumberOfVisits_ApplicationUserId",
                table: "NumberOfVisits",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NumberOfVisits_AspNetUsers_ApplicationUserId",
                table: "NumberOfVisits",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NumberOfVisits_AspNetUsers_ApplicationUserId",
                table: "NumberOfVisits");

            migrationBuilder.DropIndex(
                name: "IX_NumberOfVisits_ApplicationUserId",
                table: "NumberOfVisits");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "NumberOfVisits");
        }
    }
}
