using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Models
{
    public class SectionSchedule
    {
        [Key]
        public int IDSS { get; set; }
        [ForeignKey("IDSection")]
        public Section section { get; set; }
        [ForeignKey("IDScedule")]
        public Schedule schedule { get; set; }
    }
}