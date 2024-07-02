using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scheduler.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    IDCRS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CRS_NO = table.Column<int>(type: "int", nullable: false),
                    CRS_CR_HOURS = table.Column<int>(type: "int", nullable: false),
                    CRS_A_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CRS_SPEC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CRS_ACTIVE = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.IDCRS);
                });

            migrationBuilder.CreateTable(
                name: "DegreeProgressPlans",
                columns: table => new
                {
                    IDDegreeProgressPlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    College = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeProgressPlans", x => x.IDDegreeProgressPlan);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    IdInstructor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Job_ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.IdInstructor);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlans",
                columns: table => new
                {
                    IdStudyPlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    College = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlans", x => x.IdStudyPlan);
                });

            migrationBuilder.CreateTable(
                name: "DegreeProgresContents",
                columns: table => new
                {
                    IdDC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDCRS = table.Column<int>(type: "int", nullable: false),
                    IDDegreeProgressPlan = table.Column<int>(type: "int", nullable: false),
                    SPEC_CODE = table.Column<int>(type: "int", nullable: false),
                    SMST_NO = table.Column<int>(type: "int", nullable: false),
                    SPEC_YYT = table.Column<int>(type: "int", nullable: false),
                    SPEC_LVL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeProgresContents", x => x.IdDC);
                    table.ForeignKey(
                        name: "FK_DegreeProgresContents_Courses_IDCRS",
                        column: x => x.IDCRS,
                        principalTable: "Courses",
                        principalColumn: "IDCRS",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DegreeProgresContents_DegreeProgressPlans_IDDegreeProgressPlan",
                        column: x => x.IDDegreeProgressPlan,
                        principalTable: "DegreeProgressPlans",
                        principalColumn: "IDDegreeProgressPlan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    IDSection = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionNumber = table.Column<int>(type: "int", nullable: false),
                    Hall = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start_Sunday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Sunday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Start_Monday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Monday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Start_Tuesday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Tuesday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Start_Wednesday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Wednesday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Start_Thursday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Thursday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseIDCRS = table.Column<int>(type: "int", nullable: false),
                    InstructorsIdInstructor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.IDSection);
                    table.ForeignKey(
                        name: "FK_Sections_Courses_courseIDCRS",
                        column: x => x.courseIDCRS,
                        principalTable: "Courses",
                        principalColumn: "IDCRS",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Instructors_InstructorsIdInstructor",
                        column: x => x.InstructorsIdInstructor,
                        principalTable: "Instructors",
                        principalColumn: "IdInstructor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanContents",
                columns: table => new
                {
                    IdPlanContent = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDCRS = table.Column<int>(type: "int", nullable: false),
                    IdStudyPlan = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<int>(type: "int", nullable: false),
                    prerequisite = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanContents", x => x.IdPlanContent);
                    table.ForeignKey(
                        name: "FK_PlanContents_Courses_IDCRS",
                        column: x => x.IDCRS,
                        principalTable: "Courses",
                        principalColumn: "IDCRS",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanContents_StudyPlans_IdStudyPlan",
                        column: x => x.IdStudyPlan,
                        principalTable: "StudyPlans",
                        principalColumn: "IdStudyPlan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    KeyStudent = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Student = table.Column<int>(type: "int", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studyPlanIdStudyPlan = table.Column<int>(type: "int", nullable: false),
                    degreeProgressPlanIDDegreeProgressPlan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.KeyStudent);
                    table.ForeignKey(
                        name: "FK_Students_DegreeProgressPlans_degreeProgressPlanIDDegreeProgressPlan",
                        column: x => x.degreeProgressPlanIDDegreeProgressPlan,
                        principalTable: "DegreeProgressPlans",
                        principalColumn: "IDDegreeProgressPlan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_StudyPlans_studyPlanIdStudyPlan",
                        column: x => x.studyPlanIdStudyPlan,
                        principalTable: "StudyPlans",
                        principalColumn: "IdStudyPlan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    IDScedule = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentsKeyStudent = table.Column<int>(type: "int", nullable: false),
                    Approv_Schedule = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.IDScedule);
                    table.ForeignKey(
                        name: "FK_Schedules_Students_studentsKeyStudent",
                        column: x => x.studentsKeyStudent,
                        principalTable: "Students",
                        principalColumn: "KeyStudent",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "SectionSchedules",
                columns: table => new
                {
                    IDSS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDSection = table.Column<int>(type: "int", nullable: false),
                    IDScedule = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionSchedules", x => x.IDSS);
                    table.ForeignKey(
                        name: "FK_SectionSchedules_Schedules_IDScedule",
                        column: x => x.IDScedule,
                        principalTable: "Schedules",
                        principalColumn: "IDScedule",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionSchedules_Sections_IDSection",
                        column: x => x.IDSection,
                        principalTable: "Sections",
                        principalColumn: "IDSection",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DegreeProgresContents_IDCRS",
                table: "DegreeProgresContents",
                column: "IDCRS");

            migrationBuilder.CreateIndex(
                name: "IX_DegreeProgresContents_IDDegreeProgressPlan",
                table: "DegreeProgresContents",
                column: "IDDegreeProgressPlan");

            migrationBuilder.CreateIndex(
                name: "IX_PlanContents_IDCRS",
                table: "PlanContents",
                column: "IDCRS");

            migrationBuilder.CreateIndex(
                name: "IX_PlanContents_IdStudyPlan",
                table: "PlanContents",
                column: "IdStudyPlan");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_studentsKeyStudent",
                table: "Schedules",
                column: "studentsKeyStudent");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_courseIDCRS",
                table: "Sections",
                column: "courseIDCRS");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_InstructorsIdInstructor",
                table: "Sections",
                column: "InstructorsIdInstructor");

            migrationBuilder.CreateIndex(
                name: "IX_SectionSchedules_IDScedule",
                table: "SectionSchedules",
                column: "IDScedule");

            migrationBuilder.CreateIndex(
                name: "IX_SectionSchedules_IDSection",
                table: "SectionSchedules",
                column: "IDSection");

            migrationBuilder.CreateIndex(
                name: "IX_Students_degreeProgressPlanIDDegreeProgressPlan",
                table: "Students",
                column: "degreeProgressPlanIDDegreeProgressPlan");

            migrationBuilder.CreateIndex(
                name: "IX_Students_studyPlanIdStudyPlan",
                table: "Students",
                column: "studyPlanIdStudyPlan");

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
                name: "DegreeProgresContents");

            migrationBuilder.DropTable(
                name: "PlanContents");

            migrationBuilder.DropTable(
                name: "SectionSchedules");

            migrationBuilder.DropTable(
                name: "StudentsProgress");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "DegreeProgressPlans");

            migrationBuilder.DropTable(
                name: "StudyPlans");
        }
    }
}
