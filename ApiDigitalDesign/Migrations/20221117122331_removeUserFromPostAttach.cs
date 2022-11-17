using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDigitalDesign.Migrations
{
    public partial class removeUserFromPostAttach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttaches_Users_UserId",
                table: "PostAttaches");

            migrationBuilder.DropIndex(
                name: "IX_PostAttaches_UserId",
                table: "PostAttaches");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostAttaches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "PostAttaches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PostAttaches_UserId",
                table: "PostAttaches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostAttaches_Users_UserId",
                table: "PostAttaches",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
