using Microsoft.EntityFrameworkCore.Migrations;

namespace TM470.Data.Migrations
{
    public partial class eventtypechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventType_Venue_VenueId",
                table: "EventType");

            migrationBuilder.DropIndex(
                name: "IX_EventType_VenueId",
                table: "EventType");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "EventType");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "EventType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventType_CompanyId",
                table: "EventType",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventType_Company_CompanyId",
                table: "EventType",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventType_Company_CompanyId",
                table: "EventType");

            migrationBuilder.DropIndex(
                name: "IX_EventType_CompanyId",
                table: "EventType");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "EventType");

            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "EventType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventType_VenueId",
                table: "EventType",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventType_Venue_VenueId",
                table: "EventType",
                column: "VenueId",
                principalTable: "Venue",
                principalColumn: "VenueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
