using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChadApi.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new
                {
                    Id = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>("varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>("varchar(256) CHARACTER SET utf8mb4", maxLength: 256,
                        nullable: true),
                    ConcurrencyStamp = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new
                {
                    Id = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    FriendlyName = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: false),
                    Role = table.Column<int>("int", nullable: false),
                    UserName = table.Column<string>("varchar(256) CHARACTER SET utf8mb4", maxLength: 256,
                        nullable: true),
                    NormalizedUserName = table.Column<string>("varchar(256) CHARACTER SET utf8mb4", maxLength: 256,
                        nullable: true),
                    Email = table.Column<string>("varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>("varchar(256) CHARACTER SET utf8mb4", maxLength: 256,
                        nullable: true),
                    EmailConfirmed = table.Column<bool>("tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true),
                    SecurityStamp = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumber = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>("tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>("tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>("datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>("tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>("int", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

            migrationBuilder.CreateTable(
                "Configs",
                table => new
                {
                    Type = table.Column<string>("varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Value = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Configs", x => x.Type); });

            migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ClaimType = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true)
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
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ClaimType = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true)
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
                    LoginProvider = table.Column<string>("varchar(128) CHARACTER SET utf8mb4", maxLength: 128,
                        nullable: false),
                    ProviderKey = table.Column<string>("varchar(128) CHARACTER SET utf8mb4", maxLength: 128,
                        nullable: false),
                    ProviderDisplayName = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false)
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
                    UserId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    RoleId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false)
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
                    UserId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    LoginProvider = table.Column<string>("varchar(128) CHARACTER SET utf8mb4", maxLength: 128,
                        nullable: false),
                    Name = table.Column<string>("varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Value = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: true)
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
                "Classes",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    DirectorId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        "FK_Classes_AspNetUsers_DirectorId",
                        x => x.DirectorId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Courses",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Description = table.Column<string>("varchar(128) CHARACTER SET utf8mb4", maxLength: 128,
                        nullable: false),
                    DirectorId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        "FK_Courses_AspNetUsers_DirectorId",
                        x => x.DirectorId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Messages",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>("varchar(256) CHARACTER SET utf8mb4", maxLength: 256,
                        nullable: false),
                    Time = table.Column<DateTime>("datetime(6)", nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    SenderId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ReceiverId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        "FK_Messages_AspNetUsers_ReceiverId",
                        x => x.ReceiverId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Messages_AspNetUsers_SenderId",
                        x => x.SenderId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Resources",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Content = table.Column<byte[]>("longblob", maxLength: 4194304, nullable: false),
                    UploadTime = table.Column<DateTime>("datetime(6)", nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    Expired = table.Column<DateTime>("datetime(6)", nullable: false),
                    UploaderId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ContentType = table.Column<string>("longtext CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        "FK_Resources_AspNetUsers_UploaderId",
                        x => x.UploaderId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RelStudentClasses",
                table => new
                {
                    StudentId = table.Column<string>("varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ClassId = table.Column<long>("bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelStudentClasses", x => new {x.StudentId, x.ClassId});
                    table.ForeignKey(
                        "FK_RelStudentClasses_AspNetUsers_StudentId",
                        x => x.StudentId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RelStudentClasses_Classes_ClassId",
                        x => x.ClassId,
                        "Classes",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Lessons",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Index = table.Column<ushort>("smallint unsigned", nullable: false),
                    Description = table.Column<string>("varchar(128) CHARACTER SET utf8mb4", maxLength: 128,
                        nullable: false),
                    CourseId = table.Column<long>("bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        "FK_Lessons_Courses_CourseId",
                        x => x.CourseId,
                        "Courses",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RelCourseClasses",
                table => new
                {
                    CourseId = table.Column<long>("bigint", nullable: false),
                    ClassId = table.Column<long>("bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelCourseClasses", x => new {x.ClassId, x.CourseId});
                    table.ForeignKey(
                        "FK_RelCourseClasses_Classes_ClassId",
                        x => x.ClassId,
                        "Classes",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RelCourseClasses_Courses_CourseId",
                        x => x.CourseId,
                        "Courses",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RelResourceLessons",
                table => new
                {
                    LessonId = table.Column<long>("bigint", nullable: false),
                    ResourceId = table.Column<long>("bigint", nullable: false),
                    Index = table.Column<ushort>("smallint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelResourceLessons", x => new {x.ResourceId, x.LessonId});
                    table.ForeignKey(
                        "FK_RelResourceLessons_Lessons_LessonId",
                        x => x.LessonId,
                        "Lessons",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RelResourceLessons_Resources_ResourceId",
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
                "IX_Classes_DirectorId",
                "Classes",
                "DirectorId");

            migrationBuilder.CreateIndex(
                "IX_Courses_DirectorId",
                "Courses",
                "DirectorId");

            migrationBuilder.CreateIndex(
                "IX_Lessons_CourseId",
                "Lessons",
                "CourseId");

            migrationBuilder.CreateIndex(
                "IX_Messages_ReceiverId",
                "Messages",
                "ReceiverId");

            migrationBuilder.CreateIndex(
                "IX_Messages_SenderId_ReceiverId",
                "Messages",
                new[] {"SenderId", "ReceiverId"});

            migrationBuilder.CreateIndex(
                "IX_RelCourseClasses_ClassId_CourseId",
                "RelCourseClasses",
                new[] {"ClassId", "CourseId"});

            migrationBuilder.CreateIndex(
                "IX_RelCourseClasses_CourseId",
                "RelCourseClasses",
                "CourseId");

            migrationBuilder.CreateIndex(
                "IX_RelResourceLessons_LessonId_ResourceId",
                "RelResourceLessons",
                new[] {"LessonId", "ResourceId"});

            migrationBuilder.CreateIndex(
                "IX_RelStudentClasses_ClassId_StudentId",
                "RelStudentClasses",
                new[] {"ClassId", "StudentId"});

            migrationBuilder.CreateIndex(
                "IX_Resources_UploaderId",
                "Resources",
                "UploaderId");
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
                "RelCourseClasses");

            migrationBuilder.DropTable(
                "RelResourceLessons");

            migrationBuilder.DropTable(
                "RelStudentClasses");

            migrationBuilder.DropTable(
                "AspNetRoles");

            migrationBuilder.DropTable(
                "Lessons");

            migrationBuilder.DropTable(
                "Resources");

            migrationBuilder.DropTable(
                "Classes");

            migrationBuilder.DropTable(
                "Courses");

            migrationBuilder.DropTable(
                "AspNetUsers");
        }
    }
}