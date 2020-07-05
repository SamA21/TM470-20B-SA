using Microsoft.EntityFrameworkCore.Migrations;

namespace TM470.Data.Migrations
{
    public partial class companyevent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Event_CompanyId",
                table: "Event",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Company_CompanyId",
                table: "Event",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Company_CompanyId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_CompanyId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Event");
        }
    }
}
