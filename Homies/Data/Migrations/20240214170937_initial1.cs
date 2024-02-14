using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homies.Data.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId1",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganiserId1",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OrganiserId1",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "OrganiserId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganiserId",
                table: "Events",
                column: "OrganiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId",
                table: "Events",
                column: "OrganiserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganiserId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "OrganiserId",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "OrganiserId1",
                table: "Events",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganiserId1",
                table: "Events",
                column: "OrganiserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId1",
                table: "Events",
                column: "OrganiserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
