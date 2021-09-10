using Microsoft.EntityFrameworkCore.Migrations;

namespace CaloriesSmartCalulator.Data.Migrations
{
    public partial class ExtendSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CaloriesCalculationTasks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CaloriesCalculationTasks");
        }
    }
}
