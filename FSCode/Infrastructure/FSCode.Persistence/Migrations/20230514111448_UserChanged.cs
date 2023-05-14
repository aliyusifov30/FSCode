using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSCode.Persistence.Migrations
{
    public partial class UserChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "AspNetUsers",
                newName: "TGToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TGToken",
                table: "AspNetUsers",
                newName: "Token");

            migrationBuilder.AddColumn<string>(
                name: "TelegramId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
