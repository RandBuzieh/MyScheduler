using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Scheduler.Data;
using Scheduler.Models;
using Scheduler.Repositary;
using Scheduler.Services.CalculateFitness;
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
        public static ICreatePopulation _iCreatePopulation = new CreatePopulation();
        public static IFitnessCheck _IFitnessCheck = new FitnessCheck();

        public GenerateScheduleController(DBContextSystem context)
        {  
          _context = context; 
        }
        public IActionResult Index(string selectedCourses, string preferredInstructors, string preferredDays, int PreferredStartTime, int PreferredEndTime)
        {
            GenerateScheduleController.selectedCourses = JsonConvert.DeserializeObject<List<Course>>(selectedCourses);
            GenerateScheduleController.preferredInstructors = JsonConvert.DeserializeObject<List<Instructor>>(preferredInstructors);
            GenerateScheduleController.preferredDays = JsonConvert.DeserializeObject<Dictionary<string, bool>>(preferredDays);
            GenerateScheduleController.PreferredStartTime = PreferredStartTime;
            GenerateScheduleController.PreferredEndTime = PreferredEndTime;
            var population = InitializePopulation();
            population = EvaluateFitness(population);
            ViewData["possibleSchedules"] = population;

            return View();
        }
        public Dictionary<List<Section>, int> InitializePopulation()
        {
            var _findAllSectionsByCourse = _sectionsByCourse.FindAllSectionsByCourse(selectedCourses, _context);
            var population = _iCreatePopulation.InitializePopulation(_findAllSectionsByCourse, 50);
            return population;
        }
        public Dictionary<List<Section>, int> EvaluateFitness(Dictionary<List<Section>, int> population)
        {
            var evaluatedPopulation = _IFitnessCheck.CalculateFitness(population, PreferredStartTime, PreferredEndTime, preferredDays, preferredInstructors);
            return evaluatedPopulation;
        }

    }
}
