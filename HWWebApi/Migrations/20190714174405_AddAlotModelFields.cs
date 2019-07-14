using Microsoft.EntityFrameworkCore.Migrations;

namespace HWWebApi.Migrations
{
    public partial class AddAlotModelFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainUse",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Processors",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Generation",
                table: "Memories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "GPUs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MemoryId",
                table: "GPUs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "GPUs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ComputerType",
                table: "Computers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GPUs_MemoryId",
                table: "GPUs",
                column: "MemoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_GPUs_Memories_MemoryId",
                table: "GPUs",
                column: "MemoryId",
                principalTable: "Memories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GPUs_Memories_MemoryId",
                table: "GPUs");

            migrationBuilder.DropIndex(
                name: "IX_GPUs_MemoryId",
                table: "GPUs");

            migrationBuilder.DropColumn(
                name: "MainUse",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Processors");

            migrationBuilder.DropColumn(
                name: "Generation",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "GPUs");

            migrationBuilder.DropColumn(
                name: "MemoryId",
                table: "GPUs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "GPUs");

            migrationBuilder.DropColumn(
                name: "ComputerType",
                table: "Computers");
        }
    }
}
