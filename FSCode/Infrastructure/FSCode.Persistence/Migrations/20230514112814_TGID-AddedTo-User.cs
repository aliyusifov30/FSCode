using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSCode.Persistence.Migrations
{
    public partial class TGIDAddedToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TGId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TGId",
                table: "AspNetUsers");
        }
    }
}
