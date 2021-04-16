using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chad.Migrations
{
    public partial class DatetimeGenerated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadTime",
                table: "Resources",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "(datetime('now', 'localtime'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "(datetime('now', 'localtime'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadTime",
                table: "Resources",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "(datetime('now', 'localtime'))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "(datetime('now', 'localtime'))");
        }
    }
}
