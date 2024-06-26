using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scheduler.Migrations
{
    /// <inheritdoc />
    public partial class studentProgressTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentsProgress",
                columns: table => new
                {
                    IdProgress = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mark = table.Column<float>(type: "real", nullable: false),
                    StudentKeyStudent = table.Column<int>(type: "int", nullable: false),
                    courseIDCRS = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsProgress", x => x.IdProgress);
                    table.ForeignKey(
                        name: "FK_StudentsProgress_Courses_courseIDCRS",
                        column: x => x.courseIDCRS,
                        principalTable: "Courses",
                        principalColumn: "IDCRS",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsProgress_Students_StudentKeyStudent",
                        column: x => x.StudentKeyStudent,
                        principalTable: "Students",
                        principalColumn: "KeyStudent",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsProgress_courseIDCRS",
                table: "StudentsProgress",
                column: "courseIDCRS");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsProgress_StudentKeyStudent",
                table: "StudentsProgress",
                column: "StudentKeyStudent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsProgress");
        }
    }
}
