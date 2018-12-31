using Microsoft.EntityFrameworkCore.Migrations;

namespace HWWebApi.Migrations
{
    public partial class renamememories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_memories_Computers_ComputerId",
                table: "memories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_memories",
                table: "memories");

            migrationBuilder.RenameTable(
                name: "memories",
                newName: "Memories");

            migrationBuilder.RenameIndex(
                name: "IX_memories_ComputerId",
                table: "Memories",
                newName: "IX_Memories_ComputerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memories",
                table: "Memories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_Computers_ComputerId",
                table: "Memories",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_Computers_ComputerId",
                table: "Memories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memories",
                table: "Memories");

            migrationBuilder.RenameTable(
                name: "Memories",
                newName: "memories");

            migrationBuilder.RenameIndex(
                name: "IX_Memories_ComputerId",
                table: "memories",
                newName: "IX_memories_ComputerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_memories",
                table: "memories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_memories_Computers_ComputerId",
                table: "memories",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
