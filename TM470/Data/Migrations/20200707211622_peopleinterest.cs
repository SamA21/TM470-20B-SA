using Microsoft.EntityFrameworkCore.Migrations;

namespace TM470.Data.Migrations
{
    public partial class peopleinterest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterestLevel",
                columns: table => new
                {
                    InterestLevelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestLevel", x => x.InterestLevelId);
                });

            migrationBuilder.CreateTable(
                name: "PeopleInterest",
                columns: table => new
                {
                    PeopleInterestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    InterestLeveld = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleInterest", x => x.PeopleInterestId);
                    table.ForeignKey(
                        name: "FK_PeopleInterest_InterestLevel_InterestLeveld",
                        column: x => x.InterestLeveld,
                        principalTable: "InterestLevel",
                        principalColumn: "InterestLevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeopleInterest_InterestLeveld",
                table: "PeopleInterest",
                column: "InterestLeveld");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeopleInterest");

            migrationBuilder.DropTable(
                name: "InterestLevel");
        }
    }
}
