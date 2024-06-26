using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class StudentsProgress
    {
        [Key]
        [Required]
        public int IdProgress { get; set; }
        public float Mark { get; set; }

        public Student Student { get; set; }
        public Course course { get; set; }
    }
}
