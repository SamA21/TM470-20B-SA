using Microsoft.EntityFrameworkCore.Migrations;

namespace TM470.Data.Migrations
{
    public partial class eventtointerest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Eventld",
                table: "PeopleInterest",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PeopleInterest_Eventld",
                table: "PeopleInterest",
                column: "Eventld");

            migrationBuilder.AddForeignKey(
                name: "FK_PeopleInterest_Event_Eventld",
                table: "PeopleInterest",
                column: "Eventld",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeopleInterest_Event_Eventld",
                table: "PeopleInterest");

            migrationBuilder.DropIndex(
                name: "IX_PeopleInterest_Eventld",
                table: "PeopleInterest");

            migrationBuilder.DropColumn(
                name: "Eventld",
                table: "PeopleInterest");
        }
    }
}
