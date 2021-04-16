using Microsoft.EntityFrameworkCore.Migrations;

namespace Chad.Migrations
{
    public partial class UseComplexKeyOnRelations2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RelStudentClasses",
                table: "RelStudentClasses");

            migrationBuilder.DropIndex(
                name: "IX_RelStudentClasses_StudentId",
                table: "RelStudentClasses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RelStudentClasses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RelCourseClasses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelStudentClasses",
                table: "RelStudentClasses",
                columns: new[] { "StudentId", "ClassId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RelStudentClasses",
                table: "RelStudentClasses");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "RelStudentClasses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "RelCourseClasses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelStudentClasses",
                table: "RelStudentClasses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RelStudentClasses_StudentId",
                table: "RelStudentClasses",
                column: "StudentId");
        }
    }
}
