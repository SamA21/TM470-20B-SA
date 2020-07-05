using Microsoft.EntityFrameworkCore.Migrations;

namespace TM470.Data.Migrations
{
    public partial class Capacity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capcity",
                table: "Venue");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Venue",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Venue");

            migrationBuilder.AddColumn<int>(
                name: "Capcity",
                table: "Venue",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
