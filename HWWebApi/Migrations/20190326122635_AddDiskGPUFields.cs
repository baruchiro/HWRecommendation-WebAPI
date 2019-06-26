using Microsoft.EntityFrameworkCore.Migrations;

namespace HWWebApi.Migrations
{
    public partial class AddDiskGPUFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GPUs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Processor",
                table: "GPUs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Disks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "GPUs");

            migrationBuilder.DropColumn(
                name: "Processor",
                table: "GPUs");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Disks");
        }
    }
}
