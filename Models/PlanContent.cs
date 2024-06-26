using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Models
{
    public class PlanContent
    {
        [Key]
        public int IdPlanContent { get; set; }

        [ForeignKey("IDCRS")]
        public Course course { get; set; }

        [ForeignKey("IdStudyPlan")]
        public StudyPlan StudyPlan { get; set; }
        public int code { get; set; }
        public int? prerequisite { get; set; }
    }
}
