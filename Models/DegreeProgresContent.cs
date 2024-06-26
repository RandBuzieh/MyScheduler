using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Models
{    public class DegreeProgresContent

    {
        [Key]
        public int IdDC { get; set; }

        [ForeignKey("IDCRS")]
        public Course course { get; set; }

        [ForeignKey("IDDegreeProgressPlan")]
        public DegreeProgressPlan DegreeProgressPlan { get; set; }

        //[StringLength(4, ErrorMessage = "Spec code cannot be longer than 4 number .")]
        public int SPEC_CODE { get; set; }
        //[StringLength(1, ErrorMessage = "Smster number cannot be longer than 1 number .")]
        public int SMST_NO { get; set; }
        //[StringLength(5, ErrorMessage = "cannot be longer than 5 number .")]
        public int SPEC_YYT { get; set; }
        //[StringLength(1, ErrorMessage = "cannot be longer than 1 number .")]
        public int SPEC_LVL { get; set; }

    }
}
