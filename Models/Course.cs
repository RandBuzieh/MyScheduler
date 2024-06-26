using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Course
    {
        [Key]
        [Required]
        public int IDCRS { get; set; }
        [Required]
        //[StringLength(7, ErrorMessage = "Course number cannot be longer than 7 number.")]
        public int CRS_NO { get; set; }
        [Required]
        //[StringLength(2, ErrorMessage = "Course hours cannot be longer than 2 number.")]
        public int CRS_CR_HOURS { get; set; }
        [Required]
        //[StringLength(60, ErrorMessage = "Course name cannot be longer than 60 characters.")]
        public string CRS_A_NAME{ get; set; }
        //[StringLength(4, ErrorMessage = "Course Spec cannot be longer than 4 characters .")]
        public string CRS_SPEC {  get; set; }
        [Required]
        //[StringLength(1, ErrorMessage = "Course active cannot be longer than 1 characters .")]
        public int CRS_ACTIVE { get; set; }
        public string Type {  get; set; }
        public ICollection <DegreeProgresContent> degree_Courses { get; set; }
        public ICollection<PlanContent> plan_content { get; set; }
        public ICollection<Progress> progresses { get; set; }

        //public Progress Progress { get; set; }

    }
}
