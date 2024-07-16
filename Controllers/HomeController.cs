using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduler.Data;
using Scheduler.Models;
using System.Diagnostics;
using System.Linq;

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
        public static List<Section> availableSections = new List<Section>();

        public static List<Course> selectedCourses = new List<Course>();
        public static List<Instructor> selectedInstructors = new List<Instructor>();

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

             selectedCourses = studyPlanCourses.Where(c => selectedCourseIds.Contains(c.IDCRS)).ToList();
            
            return Redirect("DisplaySchedules");
        }
        //[HttpGet]
        //public async Task<IActionResult> ChooseInstructors()
        //{
        //    availableSections = _context.Sections
        //        .Where(dc => selectedCourses.Contains(dc.course))
        //        .Include(dc => dc.Instructors)
        //        .Include(dc => dc.course)
        //        .ToList();

        //    ViewBag.availableSections = availableSections;
        //    ViewBag.selectedCourses = selectedCourses;




        //    return View();
        //}
        //[HttpPost]
        //public IActionResult ChooseInstructors(List<int> selectedInstructorsID)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    selectedInstructors = availableSections.Where(c => selectedInstructorsID.Contains(c.Instructors.IdInstructor))
        //        .Select(c => c.Instructors)
        //        .ToList();
        //    ViewBag.selectedInstructors = selectedInstructors;

        //    return View();
        //}

        public async Task MakeScheduler()
        {
            ready = false;
            possibleSchedules = GenerateSchedules(selectedCourses);
            if (possibleSchedules.Count() != 0)
            { ready = true; }
            else
            { ready = false; }
        }

        public List<List<Section>> GenerateSchedules([FromBody] List<Course> courses)
        {
            var sectionsByCourse = new Dictionary<int, List<Section>>();

            // Retrieve all sections for each course
            foreach (var course in courses)
            {
                var sections = _context.Sections.Include(s => s.Instructors).Include(s => s.course)
                                       .Where(s => s.course.IDCRS == course.IDCRS && s.Status == "open")
                                       .ToList();
                sectionsByCourse[course.IDCRS] = sections;
            }

            // Generate all possible schedules
            var allSchedules = GenerateAllSchedules(sectionsByCourse);

            // Return the schedules
            return allSchedules;
        }

        private List<List<Section>> GenerateAllSchedules(Dictionary<int, List<Section>> sectionsByCourse)
        {
            var allSchedules = new List<List<Section>>();

            // Get all possible combinations of sections
            var sectionCombinations = GetCombinations(sectionsByCourse.Values.ToList());

            // Filter out invalid schedules with conflicting sections
            foreach (var combination in sectionCombinations)
            {
                if (!HasConflict(combination))
                {
                    allSchedules.Add(combination);
                }
            }

            return allSchedules;
        }

        private List<List<Section>> GetCombinations(List<List<Section>> lists)
        {
            var result = new List<List<Section>>();

            if (lists.Count == 0)
                return result;

            if (lists.Count == 1)
            {
                foreach (var item in lists[0])
                {
                    result.Add(new List<Section> { item });
                }
                return result;
            }

            var firstList = lists[0];
            var restCombinations = GetCombinations(lists.Skip(1).ToList());

            foreach (var item in firstList)
            {
                foreach (var combination in restCombinations)
                {
                    var newList = new List<Section> { item };
                    newList.AddRange(combination);
                    result.Add(newList);
                }
            }

            return result;
        }

        private bool HasConflict(List<Section> sections)
        {
            foreach (var section1 in sections)
            {
                foreach (var section2 in sections)
                {
                    if (section1 != section2 && AreSectionsConflicting(section1, section2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool AreSectionsConflicting(Section section1, Section section2)
        {
            // Check conflicts for each day
            return IsTimeOverlap(section1.Start_Sunday, section1.End_Sunday, section2.Start_Sunday, section2.End_Sunday) ||
                   IsTimeOverlap(section1.Start_Monday, section1.End_Monday, section2.Start_Monday, section2.End_Monday) ||
                   IsTimeOverlap(section1.Start_Tuesday, section1.End_Tuesday, section2.Start_Tuesday, section2.End_Tuesday) ||
                   IsTimeOverlap(section1.Start_Wednesday, section1.End_Wednesday, section2.Start_Wednesday, section2.End_Wednesday) ||
                   IsTimeOverlap(section1.Start_Thursday, section1.End_Thursday, section2.Start_Thursday, section2.End_Thursday);
        }

        private bool IsTimeOverlap(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2)
        {
            if (start1 == null || end1 == null || start2 == null || end2 == null)
                return false;

            return start1 < end2 && start2 < end1;
        }

        public async Task<IActionResult> DisplaySchedules()
        {
            MakeScheduler();

            ViewData["possibleSchedules"] = possibleSchedules;

            return View();
        }
    }
}