using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chad.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new
                {
                    Id = table.Column<string>("TEXT", nullable: false),
                    Name = table.Column<string>("TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>("TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new
                {
                    Id = table.Column<string>("TEXT", nullable: false),
                    FriendlyName = table.Column<string>("TEXT", nullable: false),
                    Role = table.Column<byte>("INTEGER", nullable: false),
                    Gender = table.Column<byte>("INTEGER", nullable: false),
                    UserName = table.Column<string>("TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>("TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>("TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>("TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>("INTEGER", nullable: false),
                    PasswordHash = table.Column<string>("TEXT", nullable: true),
                    SecurityStamp = table.Column<string>("TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>("TEXT", nullable: true),
                    PhoneNumber = table.Column<string>("TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>("INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>("INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>("INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>("INTEGER", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

            migrationBuilder.CreateTable(
                "Configs",
                table => new
                {
                    Type = table.Column<string>("TEXT", maxLength: 32, nullable: false),
                    Value = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Configs", x => x.Type); });

            migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new
                {
                    Id = table.Column<int>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>("TEXT", nullable: false),
                    ClaimType = table.Column<string>("TEXT", nullable: true),
                    ClaimValue = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new
                {
                    Id = table.Column<int>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>("TEXT", nullable: false),
                    ClaimType = table.Column<string>("TEXT", nullable: true),
                    ClaimValue = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUserClaims_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new
                {
                    LoginProvider = table.Column<string>("TEXT", nullable: false),
                    ProviderKey = table.Column<string>("TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>("TEXT", nullable: true),
                    UserId = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new {x.LoginProvider, x.ProviderKey});
                    table.ForeignKey(
                        "FK_AspNetUserLogins_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new
                {
                    UserId = table.Column<string>("TEXT", nullable: false),
                    RoleId = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new {x.UserId, x.RoleId});
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new
                {
                    UserId = table.Column<string>("TEXT", nullable: false),
                    LoginProvider = table.Column<string>("TEXT", nullable: false),
                    Name = table.Column<string>("TEXT", nullable: false),
                    Value = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new {x.UserId, x.LoginProvider, x.Name});
                    table.ForeignKey(
                        "FK_AspNetUserTokens_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Courses",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>("TEXT", maxLength: 32, nullable: false),
                    Description = table.Column<string>("TEXT", maxLength: 128, nullable: false),
                    DirectorId = table.Column<Guid>("TEXT", nullable: false),
                    DirectorId1 = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        "FK_Courses_AspNetUsers_DirectorId1",
                        x => x.DirectorId1,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "DbClass",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>("TEXT", maxLength: 32, nullable: false),
                    DirectorId = table.Column<Guid>("TEXT", nullable: false),
                    DirectorId1 = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbClass", x => x.Id);
                    table.ForeignKey(
                        "FK_DbClass_AspNetUsers_DirectorId1",
                        x => x.DirectorId1,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Messages",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>("TEXT", maxLength: 256, nullable: false),
                    Time = table.Column<DateTime>("TEXT", nullable: false),
                    SenderId1 = table.Column<string>("TEXT", nullable: true),
                    ReceiverId1 = table.Column<string>("TEXT", nullable: true),
                    SenderId = table.Column<Guid>("TEXT", nullable: false),
                    ReceiverId = table.Column<Guid>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        "FK_Messages_AspNetUsers_ReceiverId1",
                        x => x.ReceiverId1,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Messages_AspNetUsers_SenderId1",
                        x => x.SenderId1,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Resources",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>("TEXT", maxLength: 32, nullable: false),
                    Content = table.Column<byte[]>("BLOB", maxLength: 4194304, nullable: false),
                    UploadTime = table.Column<DateTime>("TEXT", nullable: false),
                    Expired = table.Column<DateTime>("TEXT", nullable: false),
                    UploaderId1 = table.Column<string>("TEXT", nullable: true),
                    UploaderId = table.Column<Guid>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        "FK_Resources_AspNetUsers_UploaderId1",
                        x => x.UploaderId1,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "DbLesson",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>("TEXT", maxLength: 32, nullable: false),
                    Index = table.Column<ushort>("INTEGER", nullable: false),
                    Description = table.Column<string>("TEXT", maxLength: 128, nullable: false),
                    CourseId = table.Column<long>("INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLesson", x => x.Id);
                    table.ForeignKey(
                        "FK_DbLesson_Courses_CourseId",
                        x => x.CourseId,
                        "Courses",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RelCourseClass",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<long>("INTEGER", nullable: false),
                    ClassId = table.Column<long>("INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelCourseClass", x => x.Id);
                    table.ForeignKey(
                        "FK_RelCourseClass_Courses_CourseId",
                        x => x.CourseId,
                        "Courses",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RelCourseClass_DbClass_ClassId",
                        x => x.ClassId,
                        "DbClass",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RelStudentClass",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId1 = table.Column<string>("TEXT", nullable: true),
                    StudentId = table.Column<Guid>("TEXT", nullable: false),
                    ClassId = table.Column<long>("INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelStudentClass", x => x.Id);
                    table.ForeignKey(
                        "FK_RelStudentClass_AspNetUsers_StudentId1",
                        x => x.StudentId1,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_RelStudentClass_DbClass_ClassId",
                        x => x.ClassId,
                        "DbClass",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RelResourceLesson",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LessonId = table.Column<long>("INTEGER", nullable: false),
                    ResourceId = table.Column<long>("INTEGER", nullable: false),
                    Index = table.Column<ushort>("INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelResourceLesson", x => x.Id);
                    table.ForeignKey(
                        "FK_RelResourceLesson_DbLesson_LessonId",
                        x => x.LessonId,
                        "DbLesson",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RelResourceLesson_Resources_ResourceId",
                        x => x.ResourceId,
                        "Resources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_AspNetRoleClaims_RoleId",
                "AspNetRoleClaims",
                "RoleId");

            migrationBuilder.CreateIndex(
                "RoleNameIndex",
                "AspNetRoles",
                "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_AspNetUserClaims_UserId",
                "AspNetUserClaims",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserLogins_UserId",
                "AspNetUserLogins",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserRoles_RoleId",
                "AspNetUserRoles",
                "RoleId");

            migrationBuilder.CreateIndex(
                "EmailIndex",
                "AspNetUsers",
                "NormalizedEmail");

            migrationBuilder.CreateIndex(
                "UserNameIndex",
                "AspNetUsers",
                "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Courses_DirectorId1",
                "Courses",
                "DirectorId1");

            migrationBuilder.CreateIndex(
                "IX_DbClass_DirectorId1",
                "DbClass",
                "DirectorId1");

            migrationBuilder.CreateIndex(
                "IX_DbLesson_CourseId",
                "DbLesson",
                "CourseId");

            migrationBuilder.CreateIndex(
                "IX_Messages_ReceiverId1",
                "Messages",
                "ReceiverId1");

            migrationBuilder.CreateIndex(
                "IX_Messages_SenderId_ReceiverId",
                "Messages",
                new[] {"SenderId", "ReceiverId"});

            migrationBuilder.CreateIndex(
                "IX_Messages_SenderId1",
                "Messages",
                "SenderId1");

            migrationBuilder.CreateIndex(
                "IX_RelCourseClass_ClassId_CourseId",
                "RelCourseClass",
                new[] {"ClassId", "CourseId"});

            migrationBuilder.CreateIndex(
                "IX_RelCourseClass_CourseId",
                "RelCourseClass",
                "CourseId");

            migrationBuilder.CreateIndex(
                "IX_RelResourceLesson_LessonId_ResourceId",
                "RelResourceLesson",
                new[] {"LessonId", "ResourceId"});

            migrationBuilder.CreateIndex(
                "IX_RelResourceLesson_ResourceId",
                "RelResourceLesson",
                "ResourceId");

            migrationBuilder.CreateIndex(
                "IX_RelStudentClass_ClassId_StudentId",
                "RelStudentClass",
                new[] {"ClassId", "StudentId"});

            migrationBuilder.CreateIndex(
                "IX_RelStudentClass_StudentId1",
                "RelStudentClass",
                "StudentId1");

            migrationBuilder.CreateIndex(
                "IX_Resources_UploaderId1",
                "Resources",
                "UploaderId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "AspNetRoleClaims");

            migrationBuilder.DropTable(
                "AspNetUserClaims");

            migrationBuilder.DropTable(
                "AspNetUserLogins");

            migrationBuilder.DropTable(
                "AspNetUserRoles");

            migrationBuilder.DropTable(
                "AspNetUserTokens");

            migrationBuilder.DropTable(
                "Configs");

            migrationBuilder.DropTable(
                "Messages");

            migrationBuilder.DropTable(
                "RelCourseClass");

            migrationBuilder.DropTable(
                "RelResourceLesson");

            migrationBuilder.DropTable(
                "RelStudentClass");

            migrationBuilder.DropTable(
                "AspNetRoles");

            migrationBuilder.DropTable(
                "DbLesson");

            migrationBuilder.DropTable(
                "Resources");

            migrationBuilder.DropTable(
                "DbClass");

            migrationBuilder.DropTable(
                "Courses");

            migrationBuilder.DropTable(
                "AspNetUsers");
        }
    }
}