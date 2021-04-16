using Microsoft.EntityFrameworkCore.Migrations;

namespace Chad.Migrations
{
    public partial class UseComplexKeyOnRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RelResourceLessons",
                table: "RelResourceLessons");

            migrationBuilder.DropIndex(
                name: "IX_RelResourceLessons_ResourceId",
                table: "RelResourceLessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelCourseClasses",
                table: "RelCourseClasses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RelResourceLessons");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RelCourseClasses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelResourceLessons",
                table: "RelResourceLessons",
                columns: new[] { "ResourceId", "LessonId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelCourseClasses",
                table: "RelCourseClasses",
                columns: new[] { "ClassId", "CourseId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RelResourceLessons",
                table: "RelResourceLessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelCourseClasses",
                table: "RelCourseClasses");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "RelResourceLessons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RelCourseClasses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelResourceLessons",
                table: "RelResourceLessons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelCourseClasses",
                table: "RelCourseClasses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RelResourceLessons_ResourceId",
                table: "RelResourceLessons",
                column: "ResourceId");
        }
    }
}
