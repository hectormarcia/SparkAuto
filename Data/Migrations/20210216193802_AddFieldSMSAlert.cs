using Microsoft.EntityFrameworkCore.Migrations;

namespace SparkAuto.Migrations
{
    public partial class AddFieldSMSAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SmsAlert",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsAlert",
                table: "AspNetUsers");
        }
    }
}
