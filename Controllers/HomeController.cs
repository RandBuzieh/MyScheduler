using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduler.Data;
using Scheduler.Models;
using System.Diagnostics;

namespace Scheduler.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContextSystem _context;
        public static Student? student;
        public List<Section> Sections { get; set; }
        public static int totalCreditHours;
        public static int semester;
        public static int year;
        public static bool ready;

        public static List<int> completedCourseIds;
        public static List<Course> studyPlanCourses = new List<Course>();
        public static List<List<Section>> possibleSchedules = new List<List<Section>>();
        private readonly ILogger<HomeController> _logger;

        public HomeController(DBContextSystem context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                student = _context.Students.FirstOrDefault(u => u.Email.Equals(userLogin.Email) && u.password.Equals(userLogin.password));

                if (student != null)
                {
                    char firstCher = student.Name.FirstOrDefault();
                    TempData["MasgName"] = $"{firstCher}";
                    TempData.Keep("MasgName");

                    return RedirectToAction("ViewCourses", "Home");
                }
                TempData["Msg"] = "Invalid email or password";
            }
            return View();

        }

        public async Task CalculateYearAndSemester()
        {

            double totalCompletedCreditHours = _context.StudentsProgress
                .Where(p => p.Student.KeyStudent == student.KeyStudent)
                .Join(_context.Courses, p => p.course.IDCRS, c => c.IDCRS, (p, c) => c.CRS_CR_HOURS)
                .Sum();


            int totalCreditHours = 132;
            int semesters = 8;
            double creditHoursPerSemester = totalCreditHours / (double)semesters;

            int currentSemester = (int)Math.Ceiling(totalCompletedCreditHours / creditHoursPerSemester);
            year = (currentSemester + 1) / 2;
            semester = (currentSemester % 2 == 0) ? 2 : 1;

            if (year == 4 && semester == 2)
            {
                float completedTraining = _context.StudentsProgress
                    .Where(p => p.Student.KeyStudent == student.KeyStudent && p.course.IDCRS == 51)
                    .Select(p => p.Mark)
                    .FirstOrDefault();

                int hours = FinishedCourses().Count();
                if (hours == 132)
                {
                    if (completedTraining >= 50.0f)
                    {
                        year = 131;
                        semester = 131;

                    }
                    year = 132;
                    semester = 132;

                }
            }
            else if (year == 0)
            {
                year = 1;
                semester = 1;
            }
            else
            {
                if (semester == 2)
                {
                    year++;
                    semester = 1;
                }
                else
                {
                    semester = 2;
                }
            }

            return;
        }
        public List<int> FinishedCourses()
        {
            // Fetch completed course IDs for the student
            var completedCourseIds = _context.StudentsProgress
                .Where(p => p.Student.KeyStudent == student.KeyStudent)
                .Select(p => p.course.IDCRS)
                .ToList();

            return completedCourseIds;
        }
        public async Task GetAvailableCoursesAsync()
        {


            var completedCourseIds = FinishedCourses();

            var availableCourses = _context.DegreeProgresContents
                .Where(dc => !completedCourseIds.Contains(dc.course.IDCRS))
                .Select(dc => dc.course)
                .Distinct()
                .ToList();


            // Remove courses that need a prerequisite
            var coursesStudentCanTake = new List<Course>();
            foreach (var course in availableCourses)
            {
                var prerequisiteIds = _context.PlanContents
                    .Where(pc => pc.course.IDCRS == course.IDCRS)
                    .Where(pc => pc.prerequisite != 0)
                    .Select(pc => pc.prerequisite)
                    .ToList();

                if (!prerequisiteIds.Any() || prerequisiteIds.All(prerequisite => completedCourseIds.Contains((int)prerequisite)))
                {
                    coursesStudentCanTake.Add(course);
                }
            }

            var studyPlanId = _context.Students
                .Where(s => s.KeyStudent == student.KeyStudent)
                .Select(s => s.studyPlan.IdStudyPlan)
                .FirstOrDefault();

            // Fetch courses from the student's study plan
            studyPlanCourses = _context.PlanContents
                .Where(pc => pc.StudyPlan.IdStudyPlan == studyPlanId)
                .Where(pc => coursesStudentCanTake.Contains(pc.course))
                .Select(pc => pc.course)
                .Distinct()
                .ToList();

           
        }

        [HttpGet]
        public async Task<IActionResult> ViewCourses()
        {
            await CalculateYearAndSemester();
            await GetAvailableCoursesAsync();
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

            var selectedCourses = studyPlanCourses.Where(c => selectedCourseIds.Contains(c.IDCRS)).ToList();
            ViewBag.Result = selectedCourses;

            return View("TestCourse");
        }
        public IActionResult TestCourse()
        {
            
            return View();
        }

    }
}