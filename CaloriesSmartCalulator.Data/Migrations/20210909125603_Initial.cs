using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaloriesSmartCalulator.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaloriesCalculationTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InProgressOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaloriesCalculationTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaloriesCalculationTaskItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaloriesCalculationTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaloriesCalculationTaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaloriesCalculationTaskItems_CaloriesCalculationTasks_CaloriesCalculationTaskId",
                        column: x => x.CaloriesCalculationTaskId,
                        principalTable: "CaloriesCalculationTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaloriesCalculationTaskItems_CaloriesCalculationTaskId",
                table: "CaloriesCalculationTaskItems",
                column: "CaloriesCalculationTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaloriesCalculationTaskItems");

            migrationBuilder.DropTable(
                name: "CaloriesCalculationTasks");
        }
    }
}
