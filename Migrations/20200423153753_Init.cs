using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DirectKeyDashboard.Migrations
{
    public partial class Init : Migration
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
                name: "CustomGroupedBarCharts",
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
                    SuperDatasetCategoryTokenKey = table.Column<string>(nullable: true),
                    Pivot = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomGroupedBarCharts", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "GroupedBarChartFloatCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomGroupedBarChartId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<float>(nullable: false),
                    Relation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupedBarChartFloatCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupedBarChartFloatCriteria_CustomGroupedBarCharts_CustomGr~",
                        column: x => x.CustomGroupedBarChartId,
                        principalTable: "CustomGroupedBarCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupedBarChartValueTokenKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomGroupedBarChartId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupedBarChartValueTokenKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupedBarChartValueTokenKeys_CustomGroupedBarCharts_CustomG~",
                        column: x => x.CustomGroupedBarChartId,
                        principalTable: "CustomGroupedBarCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BarChartFloatCriteria_CustomBarChartId",
                table: "BarChartFloatCriteria",
                column: "CustomBarChartId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupedBarChartFloatCriteria_CustomGroupedBarChartId",
                table: "GroupedBarChartFloatCriteria",
                column: "CustomGroupedBarChartId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupedBarChartValueTokenKeys_CustomGroupedBarChartId",
                table: "GroupedBarChartValueTokenKeys",
                column: "CustomGroupedBarChartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarChartFloatCriteria");

            migrationBuilder.DropTable(
                name: "GroupedBarChartFloatCriteria");

            migrationBuilder.DropTable(
                name: "GroupedBarChartValueTokenKeys");

            migrationBuilder.DropTable(
                name: "CustomBarCharts");

            migrationBuilder.DropTable(
                name: "CustomGroupedBarCharts");
        }
    }
}
