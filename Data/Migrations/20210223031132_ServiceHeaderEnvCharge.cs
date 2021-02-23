using Microsoft.EntityFrameworkCore.Migrations;

namespace SparkAuto.Migrations
{
    public partial class ServiceHeaderEnvCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EnvCharge",
                table: "ServiceHeader",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnvCharge",
                table: "ServiceHeader");
        }
    }
}
