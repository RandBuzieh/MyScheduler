using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Scheduler.Data;
using Scheduler.Models;
using Scheduler.Repositary;

namespace Scheduler.Controllers
{
    public class CollectDataController : Controller
    {
        public List<Section> Sections { get; set; }
        public static bool ready;
        public static List<int> completedCourseIds;
        public static List<Section> availableSections = new List<Section>();
        public static List<Section> selectedSections = new List<Section>();
        public List<List<Section>> possibleSchedules = new List<List<Section>>();
        public List<List<Section>> filteredPossibleSchedules = new List<List<Section>>();

        public static List<Course> selectedCourses = new List<Course>();
        public static int PreferredStartTime, PreferredEndTime;
        public static Dictionary<string, bool> preferredDays;
        public static List<Instructor> preferredInstructors = new List<Instructor>();
        private static List<Course> studyPlanCourses = new List<Course>();
        private StudentInfo _studentInfo;
        private readonly DBContextSystem _context;
        private static Student? _student;
        public CollectDataController(DBContextSystem context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginStuden userLogin)
        {
            if (ModelState.IsValid)
            {
                _student = _context.Students.FirstOrDefault(u => u.Email.Equals(userLogin.Email) && u.password.Equals(userLogin.password));

                if (_student != null)
                {
                    return RedirectToAction("ViewCourses");
                }
                TempData["Msg"] = "Invalid email or password";
            }
            return View();

        }


        [HttpGet]
        public async Task<IActionResult> ViewCourses()
        {
            var _studentInfo = new StudentInfo();
            studyPlanCourses = _studentInfo.GetAvailableCoursesAsync(_student, _context);
            ViewBag.studyPlanCourses = studyPlanCourses;
            return View();
        }
        [HttpPost]
        public IActionResult ViewCourses(List<int> selectedCourseIds)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            selectedCourses = studyPlanCourses.Where(c => selectedCourseIds.Contains(c.IDCRS)).ToList();

            return Redirect("ChooseInstructors");
        }

        [HttpGet]
        public async Task<IActionResult> ChooseInstructors()
        {
            availableSections = _context.Sections
               .Where(dc => selectedCourses.Contains(dc.course))
               .Include(dc => dc.Instructors)
               .Include(dc => dc.course)
               .ToList();
            ViewBag.selectedCourses = selectedCourses;
            ViewBag.availableSections = availableSections;

            return View();
        }
        [HttpPost]
        public IActionResult ChooseInstructors(List<int> selectedIdInstructor)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            preferredInstructors = _context.Instructors.Where(c => selectedIdInstructor.Contains(c.IdInstructor)).ToList();
            return Redirect("ChooseDays");
        }

        [HttpGet]
        public async Task<IActionResult> ChooseDays()
        {
            var ChooseDaysToCome = new Dictionary<string, bool>{
            { "Sunday - Tuesday - Thursday", false },
            { "Monday - Wednesday", false },
            };
            ViewBag.ChooseDaysToCome = ChooseDaysToCome;
            return View();
        }
        [HttpPost]
        public IActionResult ChooseDays(List<string> selectedDays)
        {
            if (!ModelState.IsValid)
            {
            return View();
            }
            preferredDays = new Dictionary<string, bool>{
            { "Sunday", false },{ "Monday", false },{ "Tuesday", false },{ "Wednesday", false },{ "Thursday", false }};
            foreach (string day in selectedDays)
            {
                if (day.Equals("Sunday - Tuesday - Thursday"))
                {
                    preferredDays["Sunday"] = true;
                    preferredDays["Tuesday"] = true;
                    preferredDays["Thursday"] = true;
                }
                if (day.Equals("Monday - Wednesday"))
                {
                    preferredDays["Monday"] = true;
                    preferredDays["Wednesday"] = true;
                }
            }
            return Redirect("ChooseTime");
        }

        [HttpGet]
        public async Task<IActionResult> ChooseTime()
        {
            var startHours = new List<int>{
                8,9,10,11
            };
            ViewBag.startHours = startHours;

            var endHours = new List<int>{
                12,13,14,15,16,17
            };
            ViewBag.endHours = endHours;
            return View();
        }
        [HttpPost]
        public IActionResult ChooseTime(int selectedStartTime , int selectedEndTime)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            PreferredStartTime = selectedStartTime;
            PreferredEndTime = selectedEndTime;
            return Redirect("SendDataToGenerate");
        }
       public IActionResult SendDataToGenerate()
       {
    return RedirectToAction("Index", "GenerateSchedule", new 
    { 
        selectedCourses = JsonConvert.SerializeObject(selectedCourses),
        preferredInstructors = JsonConvert.SerializeObject(preferredInstructors),
        preferredDays = JsonConvert.SerializeObject(preferredDays),
        PreferredStartTime, PreferredEndTime
    }
       );
       }

    }
    }
