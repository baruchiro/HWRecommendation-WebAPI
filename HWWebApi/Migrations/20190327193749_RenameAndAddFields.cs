using Microsoft.EntityFrameworkCore.Migrations;

namespace HWWebApi.Migrations
{
    public partial class RenameAndAddFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "MotherBoards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "MotherBoards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankLabel",
                table: "Memories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceLocator",
                table: "Memories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "MotherBoards");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "MotherBoards");

            migrationBuilder.DropColumn(
                name: "BankLabel",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "DeviceLocator",
                table: "Memories");
        }
    }
}
