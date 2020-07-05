using Microsoft.EntityFrameworkCore.Migrations;

namespace TM470.Data.Migrations
{
    public partial class capcity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capcity",
                table: "Venue",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventCapacity",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PeopleIntrested",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice",
                table: "Event",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TicketsSold",
                table: "Event",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capcity",
                table: "Venue");

            migrationBuilder.DropColumn(
                name: "EventCapacity",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "PeopleIntrested",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TicketsSold",
                table: "Event");
        }
    }
}
