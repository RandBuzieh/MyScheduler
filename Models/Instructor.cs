using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Instructor
    {
        [Key]
        public int IdInstructor { get; set; }
        public int Job_ID { get; set; }
        public string Name { get; set; }
        public ICollection<Section> Sections { get; set; }

    }
}
