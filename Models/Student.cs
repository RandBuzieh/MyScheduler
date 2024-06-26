using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Student
    {
        [Key]
        [Required]
        public int KeyStudent { get; set; }
        public int ID_Student {  get; set; }    
        [Required]
        [PasswordPropertyText]
        public string password {  get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public StudyPlan studyPlan { get; set; }
        public DegreeProgressPlan degreeProgressPlan { get; set; }
        public ICollection<Schedule> schedules { get; set; }
        public ICollection<StudentsProgress> progresses { get; set; }
    }
}
