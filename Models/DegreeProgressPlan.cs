using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class DegreeProgressPlan
    {
        [Key]
        [Required]
        public int IDDegreeProgressPlan { get; set; }
        public string Name { get; set; }
        public string College { get; set; }
        public string Major { get; set; }
        public ICollection<Student> Student { get; set; }
        public ICollection<DegreeProgresContent> degreeProgresContents { get; set; }

    }
}
