using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDigitalDesign.Migrations
{
    public partial class addSubscribes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscribes",
                columns: table => new
                {
                    SubscriberId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribes", x => new { x.SubscriberId, x.RecipientId });
                    table.ForeignKey(
                        name: "FK_Subscribes_Users_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscribes_Users_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscribes_RecipientId",
                table: "Subscribes",
                column: "RecipientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscribes");
        }
    }
}
