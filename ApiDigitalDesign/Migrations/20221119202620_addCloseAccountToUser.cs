using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDigitalDesign.Migrations
{
    public partial class addCloseAccountToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CloseAccount",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseAccount",
                table: "Users");
        }
    }
}
