using Scheduler.Models;
using Microsoft.EntityFrameworkCore;


namespace Scheduler.Data
{
    public class DBContextSystem : DbContext
    {
        public DBContextSystem(DbContextOptions<DBContextSystem> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<DegreeProgressPlan> degreeProgressPlans { get; set; }
        public DbSet<DegreeProgresContent> DegreeProgresContents { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<PlanContent> PlanContents { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionSchedule> sectionSchedules { get; set; }
        public DbSet<StudentsProgress> Progresses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }


}
