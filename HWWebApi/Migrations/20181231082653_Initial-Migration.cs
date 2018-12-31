using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HWWebApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotherBoards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DdrSockets = table.Column<int>(nullable: true),
                    MaxRam = table.Column<long>(nullable: true),
                    SataConnections = table.Column<int>(nullable: true),
                    Architecture = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    GHz = table.Column<long>(nullable: true),
                    NumOfCores = table.Column<int>(nullable: true),
                    Architecture = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Computers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProcessorId = table.Column<long>(nullable: true),
                    MotherBoardId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Computers_MotherBoards_MotherBoardId",
                        column: x => x.MotherBoardId,
                        principalTable: "MotherBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Computers_Processors_ProcessorId",
                        column: x => x.ProcessorId,
                        principalTable: "Processors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Disks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: true),
                    Rpm = table.Column<int>(nullable: true),
                    Capacity = table.Column<long>(nullable: true),
                    ComputerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disks_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GPUs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cores = table.Column<int>(nullable: true),
                    ComputerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GPUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GPUs_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "memories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Capacity = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Ghz = table.Column<long>(nullable: false),
                    ComputerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_memories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_memories_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Computers_MotherBoardId",
                table: "Computers",
                column: "MotherBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Computers_ProcessorId",
                table: "Computers",
                column: "ProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Disks_ComputerId",
                table: "Disks",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_GPUs_ComputerId",
                table: "GPUs",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_memories_ComputerId",
                table: "memories",
                column: "ComputerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disks");

            migrationBuilder.DropTable(
                name: "GPUs");

            migrationBuilder.DropTable(
                name: "memories");

            migrationBuilder.DropTable(
                name: "Computers");

            migrationBuilder.DropTable(
                name: "MotherBoards");

            migrationBuilder.DropTable(
                name: "Processors");
        }
    }
}
