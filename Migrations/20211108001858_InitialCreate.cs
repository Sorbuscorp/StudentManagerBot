using Microsoft.EntityFrameworkCore.Migrations;

namespace MainServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    FIO = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Group = table.Column<string>(type: "TEXT", nullable: true),
                    Marks = table.Column<string>(type: "TEXT", nullable: false),
                    FIO = table.Column<string>(type: "TEXT", nullable: true),
                    LecturerUsername = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Students_Lecturers_LecturerUsername",
                        column: x => x.LecturerUsername,
                        principalTable: "Lecturers",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_LecturerUsername",
                table: "Students",
                column: "LecturerUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Lecturers");
        }
    }
}
