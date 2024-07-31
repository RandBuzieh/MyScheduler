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
        public static List<Section> selectedSections = new List<Section>();
        public static List<int> preferredInstructors = new List<int>();

        public List<List<Section>> possibleSchedules = new List<List<Section>>();
        public List<List<Section>> filteredPossibleSchedules = new List<List<Section>>();
        public static Dictionary<string, bool> preferredDays ;

        private readonly ILogger<HomeController> _logger;
        Dictionary<int, List<Section>> sectionsByCourse= new Dictionary<int, List<Section>>();

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
            preferredInstructors = selectedIdInstructor;
            return Redirect("ChooseDays");
        }

        [HttpGet]
        public async Task<IActionResult> ChooseDays()
        {
            preferredDays = new Dictionary<string, bool>{
            { "Sunday - Tuesday - Thursday", false },
            { "Monday - Wednesday", false },
            };
            ViewBag.preferredDays = preferredDays;
            return View(); 
        }
        [HttpPost]
        public IActionResult ChooseDays (List<string> selectedDays)
        {
            foreach (string day in selectedDays)
            {
                preferredDays[day] = true;
            }
            return Redirect("DisplaySchedules");
        }
        public async Task MakeScheduler()
        {
            foreach (var course in selectedCourses)
            {
                List<Section> courseSections = new List<Section>();
                var Sections = _context.Sections
                .Where(dc => course.IDCRS == dc.course.IDCRS)
                .Include(dc => dc.Instructors)
                .Include(dc => dc.course)
                .ToList();
                sectionsByCourse.Add(course.IDCRS, Sections);
            }
            ready = false;
            possibleSchedules = GenerateAllSchedules(sectionsByCourse, preferredInstructors);
            if (possibleSchedules.Count() != 0)
            { ready = true; }
            else
            { ready = false; }
        }

        public List<List<Section>> GenerateAllSchedules(Dictionary<int, List<Section>> sectionsByCourse, List<int> preferredInstructors, double preferenceThreshold = 0.8)
        {
            var populationSize = 50;
            var generations = 1;
            var mutationRate = 0.01;

            // Initialize population
            var population = InitializePopulation(sectionsByCourse, populationSize);

            for (int generation = 0; generation < generations; generation++)
            {
                // Evaluate fitness
                var fitnessScores = EvaluateFitness(population, sectionsByCourse, preferredInstructors);

                // Selection
                var selectedSchedules = SelectSchedules(population, fitnessScores);

                // Crossover
                var offspring = Crossover(selectedSchedules);

                // Mutation
                Mutate(offspring, mutationRate, sectionsByCourse);

                // Create new population
                population = new List<List<Section>>(offspring);
            }

            // Return the best schedules meeting the preference threshold
            return population.Where(s => CalculatePreferenceMatch(s, preferredInstructors) >= preferenceThreshold).ToList();
        }

        private List<List<Section>> InitializePopulation(Dictionary<int, List<Section>> sectionsByCourse, int populationSize)
        {
            var population = new List<List<Section>>();
            var random = new Random();
            for (int i = 0; i < populationSize; i++)
            {
                var schedule = new List<Section>();
                foreach (var courseSections in sectionsByCourse.Values)
                {
                    Section section = courseSections[random.Next(courseSections.Count)];
                    schedule.Add(section);
                }

                population.Add(schedule);
            }
            return CheckForRepetition(population);
;
        }

        private List<double> EvaluateFitness(List<List<Section>> population, Dictionary<int, List<Section>> sectionsByCourse, List<int> preferredInstructors)
        {
            var fitnessScores = new List<double>();

            foreach (var schedule in population)
            {
                double fitness = 0;

                // Check for conflicts and completeness
                if (!HasConflict(schedule) && IncludesAllSelectedCourses(schedule, sectionsByCourse))
                {
                    fitness = 1; // Assign a base fitness score for valid schedules
                    fitness += schedule.Count; // Optionally, reward schedules with more courses

                    // Calculate preference match percentage
                    double preferenceMatch = CalculatePreferenceMatch(schedule, preferredInstructors);
                    fitness += preferenceMatch; // Reward schedules that match preferences
                }

                fitnessScores.Add(fitness);
            }

            return fitnessScores;
        }

        private bool IncludesAllSelectedCourses(List<Section> schedule, Dictionary<int, List<Section>> sectionsByCourse)
        {
            var courseIdsInSchedule = schedule.Select(s => s.course.IDCRS).ToHashSet();
            var allCourseIds = sectionsByCourse.Keys.ToHashSet();

            return allCourseIds.SetEquals(courseIdsInSchedule);
        }

        private double CalculatePreferenceMatch(List<Section> schedule, List<int> preferredInstructors)
        {
            int totalInstructors = schedule.Select(s => s.Instructors).Distinct().Count();
            int preferredCount = 0;
            foreach (var section in schedule)
            {
                if(preferredInstructors.Contains(section.Instructors.IdInstructor))
                {
                    preferredCount++;
                }
            }

            return totalInstructors > 0 ? (double)preferredCount / totalInstructors : 0;      
        }

        private List<List<Section>> SelectSchedules(List<List<Section>> population, List<double> fitnessScores)
        {
            var selectedSchedules = new List<List<Section>>();
            var random = new Random();

            // Example selection method: roulette wheel selection
            double totalFitness = fitnessScores.Sum();

            for (int i = 0; i < population.Count / 2; i++)
            {
                double rouletteWheelPosition = random.NextDouble() * totalFitness;
                double cumulativeFitness = 0;

                for (int j = 0; j < population.Count; j++)
                {
                    cumulativeFitness += fitnessScores[j];
                    if (cumulativeFitness >= rouletteWheelPosition)
                    {
                        selectedSchedules.Add(population[j]);
                        break;
                    }
                }
            }

            return selectedSchedules;
        }

        private List<List<Section>> Crossover(List<List<Section>> selectedSchedules)
        {
            var offspring = new List<List<Section>>();
            var random = new Random();

            for (int i = 0; i < selectedSchedules.Count / 2; i++)
            {
                var parent1 = selectedSchedules[i];
                var parent2 = selectedSchedules[selectedSchedules.Count - 1 - i];

                var child1 = new List<Section>();
                var child2 = new List<Section>();

                for (int j = 0; j < parent1.Count; j++)
                {
                    if (random.NextDouble() < 0.5)
                    {
                        child1.Add(parent1[j]);
                        child2.Add(parent2[j]);
                    }
                    else
                    {
                        child1.Add(parent2[j]);
                        child2.Add(parent1[j]);
                    }
                }

                offspring.Add(child1);
                offspring.Add(child2);
            }

            return offspring;
        }

        private void Mutate(List<List<Section>> offspring, double mutationRate, Dictionary<int, List<Section>> sectionsByCourse)
        {
            var random = new Random();

            foreach (var schedule in offspring)
            {
                if (random.NextDouble() < mutationRate)
                {
                    int index = random.Next(schedule.Count);
                    var courseId = schedule[index].course.IDCRS;
                    var courseSections = sectionsByCourse[courseId];
                    schedule[index] = courseSections[random.Next(courseSections.Count)];
                }
            }
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

        public List<List<Section>> CheckForRepetition(List<List<Section>> possibleSchedules)
        {
            List<string> populationSchedule = new List<string>();

            foreach (var schedule in possibleSchedules)
            {
                string check = "";

                foreach (var section in schedule)
                {
                    check += section.IDSection;
                }
                if (!populationSchedule.Contains(check))
                {
                    populationSchedule.Add(check);
                    filteredPossibleSchedules.Add(schedule);
                }
            }
            return filteredPossibleSchedules;
        }

        public async Task<IActionResult> DisplaySchedules()
        {
            MakeScheduler();
            ViewData["possibleSchedules"] = CheckForRepetition(possibleSchedules);
            return View();
        }
    }
}