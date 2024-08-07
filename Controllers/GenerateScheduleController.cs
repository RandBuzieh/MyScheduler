using Microsoft.AspNetCore.Mvc;
using Scheduler.Data;
using Scheduler.Models;
using Scheduler.Repositary;
using Scheduler.Services.PopulationGenerating;

namespace Scheduler.Controllers
{
    public class GenerateScheduleController : Controller
    {
        private readonly DBContextSystem _context;
        public static List<Course> selectedCourses = new List<Course>();
        public static int PreferredStartTime, PreferredEndTime;
        public static Dictionary<string, bool> preferredDays;
        public static List<Instructor> preferredInstructors = new List<Instructor>();
        public static SectionsByCourse _sectionsByCourse = new SectionsByCourse();
        public static ICreatePopulation _iCreatePopulation;

        public GenerateScheduleController(DBContextSystem context)
        {  
          _context = context; 
        }
        public IActionResult Index()
        {
            selectedCourses = ViewBag.selectedCourses;
            preferredInstructors = ViewBag.preferredInstructors;
            preferredDays = ViewBag.preferredDays;
            PreferredStartTime = ViewBag.PreferredStartTime;
            PreferredEndTime = ViewBag.PreferredEndTime;
            var _findAllSectionsByCourse = _sectionsByCourse.FindAllSectionsByCourse(selectedCourses, _context);
            var population = _iCreatePopulation.InitializePopulation(_findAllSectionsByCourse,50);
            return View();
        }
    } 
}
