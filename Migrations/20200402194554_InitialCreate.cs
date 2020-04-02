using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DirectKeyDashboard.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomBarCharts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ApiEndpoint = table.Column<string>(nullable: true),
                    ProjectionResult = table.Column<int>(nullable: false),
                    SummaryMethod = table.Column<int>(nullable: false),
                    CriterionType = table.Column<int>(nullable: false),
                    IntervalStart = table.Column<DateTime>(nullable: false),
                    IntervalEnd = table.Column<DateTime>(nullable: false),
                    CategoryTokenKey = table.Column<string>(nullable: true),
                    ValueTokenKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomBarCharts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BarChartFloatCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomBarChartId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<float>(nullable: false),
                    Relation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarChartFloatCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarChartFloatCriteria_CustomBarCharts_CustomBarChartId",
                        column: x => x.CustomBarChartId,
                        principalTable: "CustomBarCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BarChartFloatCriteria_CustomBarChartId",
                table: "BarChartFloatCriteria",
                column: "CustomBarChartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarChartFloatCriteria");

            migrationBuilder.DropTable(
                name: "CustomBarCharts");
        }
    }
}
