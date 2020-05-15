using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DirectKeyDashboard.Migrations
{
    public partial class RefactorCustomCharts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriterionType",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "SummaryMethod",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "SuperDatasetCategoryTokenKey",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "CriterionType",
                table: "CustomBarCharts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomBarCharts");

            migrationBuilder.DropColumn(
                name: "SummaryMethod",
                table: "CustomBarCharts");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectionResult",
                table: "CustomGroupedBarCharts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalStart",
                table: "CustomGroupedBarCharts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalEnd",
                table: "CustomGroupedBarCharts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "CategoryTokenKey",
                table: "CustomGroupedBarCharts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatasetTokenKey",
                table: "CustomGroupedBarCharts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelativeTimeGranularity",
                table: "CustomGroupedBarCharts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelativeTimeValue",
                table: "CustomGroupedBarCharts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SummaryMethodDescriptor",
                table: "CustomGroupedBarCharts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TimeRelative",
                table: "CustomGroupedBarCharts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CustomGroupedBarCharts",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectionResult",
                table: "CustomBarCharts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalStart",
                table: "CustomBarCharts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalEnd",
                table: "CustomBarCharts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<int>(
                name: "RelativeTimeGranularity",
                table: "CustomBarCharts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelativeTimeValue",
                table: "CustomBarCharts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SummaryMethodDescriptor",
                table: "CustomBarCharts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TimeRelative",
                table: "CustomBarCharts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CustomBarCharts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryTokenKey",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "DatasetTokenKey",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "RelativeTimeGranularity",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "RelativeTimeValue",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "SummaryMethodDescriptor",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "TimeRelative",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CustomGroupedBarCharts");

            migrationBuilder.DropColumn(
                name: "RelativeTimeGranularity",
                table: "CustomBarCharts");

            migrationBuilder.DropColumn(
                name: "RelativeTimeValue",
                table: "CustomBarCharts");

            migrationBuilder.DropColumn(
                name: "SummaryMethodDescriptor",
                table: "CustomBarCharts");

            migrationBuilder.DropColumn(
                name: "TimeRelative",
                table: "CustomBarCharts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CustomBarCharts");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectionResult",
                table: "CustomGroupedBarCharts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalStart",
                table: "CustomGroupedBarCharts",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalEnd",
                table: "CustomGroupedBarCharts",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CriterionType",
                table: "CustomGroupedBarCharts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomGroupedBarCharts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SummaryMethod",
                table: "CustomGroupedBarCharts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SuperDatasetCategoryTokenKey",
                table: "CustomGroupedBarCharts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectionResult",
                table: "CustomBarCharts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalStart",
                table: "CustomBarCharts",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalEnd",
                table: "CustomBarCharts",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CriterionType",
                table: "CustomBarCharts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomBarCharts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SummaryMethod",
                table: "CustomBarCharts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
