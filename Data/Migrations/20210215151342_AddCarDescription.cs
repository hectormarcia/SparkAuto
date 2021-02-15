using Microsoft.EntityFrameworkCore.Migrations;

namespace SparkAuto.Data.Migrations
{
    public partial class AddCarDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Car",
                type: "nvarchar(max)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Car");
        }
    }
}
