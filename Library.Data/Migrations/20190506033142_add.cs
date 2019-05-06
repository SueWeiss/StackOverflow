using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Data.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Questions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Questions");
        }
    }
}
