using Microsoft.EntityFrameworkCore.Migrations;

namespace Chad.Migrations
{
    public partial class RemovedGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Gender",
                "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                "Gender",
                "AspNetUsers",
                "INTEGER",
                nullable: false,
                defaultValue: (byte) 0);
        }
    }
}