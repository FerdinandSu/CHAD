using Microsoft.EntityFrameworkCore.Migrations;

namespace Chad.Migrations
{
    public partial class Update415 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_DirectorId1",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_DbClass_AspNetUsers_DirectorId1",
                table: "DbClass");

            migrationBuilder.DropForeignKey(
                name: "FK_DbLesson_Courses_CourseId",
                table: "DbLesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId1",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId1",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_RelCourseClass_Courses_CourseId",
                table: "RelCourseClass");

            migrationBuilder.DropForeignKey(
                name: "FK_RelCourseClass_DbClass_ClassId",
                table: "RelCourseClass");

            migrationBuilder.DropForeignKey(
                name: "FK_RelResourceLesson_DbLesson_LessonId",
                table: "RelResourceLesson");

            migrationBuilder.DropForeignKey(
                name: "FK_RelResourceLesson_Resources_ResourceId",
                table: "RelResourceLesson");

            migrationBuilder.DropForeignKey(
                name: "FK_RelStudentClass_AspNetUsers_StudentId1",
                table: "RelStudentClass");

            migrationBuilder.DropForeignKey(
                name: "FK_RelStudentClass_DbClass_ClassId",
                table: "RelStudentClass");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_AspNetUsers_UploaderId1",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_UploaderId1",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DirectorId1",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelStudentClass",
                table: "RelStudentClass");

            migrationBuilder.DropIndex(
                name: "IX_RelStudentClass_StudentId1",
                table: "RelStudentClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelResourceLesson",
                table: "RelResourceLesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelCourseClass",
                table: "RelCourseClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbLesson",
                table: "DbLesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbClass",
                table: "DbClass");

            migrationBuilder.DropIndex(
                name: "IX_DbClass_DirectorId1",
                table: "DbClass");

            migrationBuilder.DropColumn(
                name: "UploaderId1",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ReceiverId1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderId1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DirectorId1",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "RelStudentClass");

            migrationBuilder.DropColumn(
                name: "DirectorId1",
                table: "DbClass");

            migrationBuilder.RenameTable(
                name: "RelStudentClass",
                newName: "RelStudentClasses");

            migrationBuilder.RenameTable(
                name: "RelResourceLesson",
                newName: "RelResourceLessons");

            migrationBuilder.RenameTable(
                name: "RelCourseClass",
                newName: "RelCourseClasses");

            migrationBuilder.RenameTable(
                name: "DbLesson",
                newName: "Lessons");

            migrationBuilder.RenameTable(
                name: "DbClass",
                newName: "Classes");

            migrationBuilder.RenameIndex(
                name: "IX_RelStudentClass_ClassId_StudentId",
                table: "RelStudentClasses",
                newName: "IX_RelStudentClasses_ClassId_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_RelResourceLesson_ResourceId",
                table: "RelResourceLessons",
                newName: "IX_RelResourceLessons_ResourceId");

            migrationBuilder.RenameIndex(
                name: "IX_RelResourceLesson_LessonId_ResourceId",
                table: "RelResourceLessons",
                newName: "IX_RelResourceLessons_LessonId_ResourceId");

            migrationBuilder.RenameIndex(
                name: "IX_RelCourseClass_CourseId",
                table: "RelCourseClasses",
                newName: "IX_RelCourseClasses_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_RelCourseClass_ClassId_CourseId",
                table: "RelCourseClasses",
                newName: "IX_RelCourseClasses_ClassId_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_DbLesson_CourseId",
                table: "Lessons",
                newName: "IX_Lessons_CourseId");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Resources",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelStudentClasses",
                table: "RelStudentClasses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelResourceLessons",
                table: "RelResourceLessons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelCourseClasses",
                table: "RelCourseClasses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classes",
                table: "Classes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_UploaderId",
                table: "Resources",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DirectorId",
                table: "Courses",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_RelStudentClasses_StudentId",
                table: "RelStudentClasses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_DirectorId",
                table: "Classes",
                column: "DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_DirectorId",
                table: "Classes",
                column: "DirectorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_DirectorId",
                table: "Courses",
                column: "DirectorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelCourseClasses_Classes_ClassId",
                table: "RelCourseClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelCourseClasses_Courses_CourseId",
                table: "RelCourseClasses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelResourceLessons_Lessons_LessonId",
                table: "RelResourceLessons",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelResourceLessons_Resources_ResourceId",
                table: "RelResourceLessons",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelStudentClasses_AspNetUsers_StudentId",
                table: "RelStudentClasses",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelStudentClasses_Classes_ClassId",
                table: "RelStudentClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_AspNetUsers_UploaderId",
                table: "Resources",
                column: "UploaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_DirectorId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_DirectorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_RelCourseClasses_Classes_ClassId",
                table: "RelCourseClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_RelCourseClasses_Courses_CourseId",
                table: "RelCourseClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_RelResourceLessons_Lessons_LessonId",
                table: "RelResourceLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_RelResourceLessons_Resources_ResourceId",
                table: "RelResourceLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_RelStudentClasses_AspNetUsers_StudentId",
                table: "RelStudentClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_RelStudentClasses_Classes_ClassId",
                table: "RelStudentClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_AspNetUsers_UploaderId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_UploaderId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DirectorId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelStudentClasses",
                table: "RelStudentClasses");

            migrationBuilder.DropIndex(
                name: "IX_RelStudentClasses_StudentId",
                table: "RelStudentClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelResourceLessons",
                table: "RelResourceLessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelCourseClasses",
                table: "RelCourseClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classes",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_DirectorId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Resources");

            migrationBuilder.RenameTable(
                name: "RelStudentClasses",
                newName: "RelStudentClass");

            migrationBuilder.RenameTable(
                name: "RelResourceLessons",
                newName: "RelResourceLesson");

            migrationBuilder.RenameTable(
                name: "RelCourseClasses",
                newName: "RelCourseClass");

            migrationBuilder.RenameTable(
                name: "Lessons",
                newName: "DbLesson");

            migrationBuilder.RenameTable(
                name: "Classes",
                newName: "DbClass");

            migrationBuilder.RenameIndex(
                name: "IX_RelStudentClasses_ClassId_StudentId",
                table: "RelStudentClass",
                newName: "IX_RelStudentClass_ClassId_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_RelResourceLessons_ResourceId",
                table: "RelResourceLesson",
                newName: "IX_RelResourceLesson_ResourceId");

            migrationBuilder.RenameIndex(
                name: "IX_RelResourceLessons_LessonId_ResourceId",
                table: "RelResourceLesson",
                newName: "IX_RelResourceLesson_LessonId_ResourceId");

            migrationBuilder.RenameIndex(
                name: "IX_RelCourseClasses_CourseId",
                table: "RelCourseClass",
                newName: "IX_RelCourseClass_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_RelCourseClasses_ClassId_CourseId",
                table: "RelCourseClass",
                newName: "IX_RelCourseClass_ClassId_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_CourseId",
                table: "DbLesson",
                newName: "IX_DbLesson_CourseId");

            migrationBuilder.AddColumn<string>(
                name: "UploaderId1",
                table: "Resources",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId1",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderId1",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DirectorId1",
                table: "Courses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId1",
                table: "RelStudentClass",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DirectorId1",
                table: "DbClass",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelStudentClass",
                table: "RelStudentClass",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelResourceLesson",
                table: "RelResourceLesson",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelCourseClass",
                table: "RelCourseClass",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbLesson",
                table: "DbLesson",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbClass",
                table: "DbClass",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_UploaderId1",
                table: "Resources",
                column: "UploaderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId1",
                table: "Messages",
                column: "ReceiverId1");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId1",
                table: "Messages",
                column: "SenderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DirectorId1",
                table: "Courses",
                column: "DirectorId1");

            migrationBuilder.CreateIndex(
                name: "IX_RelStudentClass_StudentId1",
                table: "RelStudentClass",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_DbClass_DirectorId1",
                table: "DbClass",
                column: "DirectorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_DirectorId1",
                table: "Courses",
                column: "DirectorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DbClass_AspNetUsers_DirectorId1",
                table: "DbClass",
                column: "DirectorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DbLesson_Courses_CourseId",
                table: "DbLesson",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId1",
                table: "Messages",
                column: "ReceiverId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId1",
                table: "Messages",
                column: "SenderId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelCourseClass_Courses_CourseId",
                table: "RelCourseClass",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelCourseClass_DbClass_ClassId",
                table: "RelCourseClass",
                column: "ClassId",
                principalTable: "DbClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelResourceLesson_DbLesson_LessonId",
                table: "RelResourceLesson",
                column: "LessonId",
                principalTable: "DbLesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelResourceLesson_Resources_ResourceId",
                table: "RelResourceLesson",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelStudentClass_AspNetUsers_StudentId1",
                table: "RelStudentClass",
                column: "StudentId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelStudentClass_DbClass_ClassId",
                table: "RelStudentClass",
                column: "ClassId",
                principalTable: "DbClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_AspNetUsers_UploaderId1",
                table: "Resources",
                column: "UploaderId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
