using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Section
    {
        [Key]
        [Required]
        public int IDSection { get; set; }
        public int SectionNumber { get; set; }
        [Required]
        public string Hall {  get; set; }

        [DataType(DataType.Time)]
       
        public DateTime? Start_Sunday {  get; set; }
        [DataType(DataType.Time)]
       
        public DateTime? End_Sunday { get; set; }
        [DataType(DataType.Time)]
       
        public DateTime? Start_Monday { get; set; }
        [DataType(DataType.Time)]
      
        public DateTime? End_Monday { get; set; }
        [DataType(DataType.Time)]
      
        public DateTime? Start_Tuesday { get; set; }
        [DataType(DataType.Time)]
      
        public DateTime? End_Tuesday { get; set; }
        [DataType(DataType.Time)]

        public DateTime? Start_Wednesday { get; set; }
        [DataType(DataType.Time)]

        public DateTime? End_Wednesday { get; set; }
        [DataType(DataType.Time)]
  
        public DateTime? Start_Thursday { get; set; }
        [DataType(DataType.Time)]

        public DateTime? End_Thursday { get; set; }
        public String Status { get; set; }

        public Course course { get; set; }
        public ICollection<SectionSchedule> sectionSchedules { get; set; }
        public Instructor Instructors {  get; set; }
      
    }
}
