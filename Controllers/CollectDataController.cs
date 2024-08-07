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


        //        public async Task MakeScheduler()
        //        {

        //            ready = false;
        //            possibleSchedules = GenerateAllSchedules(sectionsByCourse, preferredInstructors);
        //            if (possibleSchedules.Count() != 0)
        //            { ready = true; }
        //            else
        //            { ready = false; }
        //        }

        //        public List<List<Section>> GenerateAllSchedules(Dictionary<int, List<Section>> sectionsByCourse, List<int> preferredInstructors, double preferenceThreshold = 1)
        //        {
        //            
        //            var generations = 1;
        //            var mutationRate = 0.01;

        //            // Initialize population
        //            var population = InitializePopulation(sectionsByCourse, populationSize);

        //            for (int generation = 0; generation < generations; generation++)
        //            {
        //                // Evaluate fitness
        //                var fitnessScores = EvaluateFitness(population, sectionsByCourse, preferredInstructors);

        //                // Selection
        //                var selectedSchedules = SelectSchedules(population, fitnessScores);

        //                // Crossover
        //                var offspring = Crossover(selectedSchedules);

        //                // Mutation
        //                Mutate(offspring, mutationRate, sectionsByCourse);

        //                // Create new population
        //                population = new List<List<Section>>(offspring);
        //            }

        //            // Return the best schedules meeting the preference threshold
        //            return population.Where(s => CalculatePreferenceMatch(s, preferredInstructors) >= preferenceThreshold).ToList();
        //        }



        //        private List<double> EvaluateFitness(List<List<Section>> population, Dictionary<int, List<Section>> sectionsByCourse, List<int> preferredInstructors)
        //        {
        //            var fitnessScores = new List<double>();

        //            foreach (var schedule in population)
        //            {
        //                double fitness = 0;

        //                // Check for conflicts and completeness
        //                if (!CheckForConflict(schedule) && IncludesAllSelectedCourses(schedule, sectionsByCourse))
        //                {
        //                    fitness = 1; // Assign a base fitness score for valid schedules
        //                    fitness += schedule.Count; // Optionally, reward schedules with more courses

        //                    // Calculate preference match percentage
        //                    double preferenceMatch = CalculatePreferenceMatch(schedule, preferredInstructors);
        //                    fitness += preferenceMatch; // Reward schedules that match preferences
        //                }

        //                fitnessScores.Add(fitness);
        //            }

        //            return fitnessScores;
        //        }

        //        private bool IncludesAllSelectedCourses(List<Section> schedule, Dictionary<int, List<Section>> sectionsByCourse)
        //        {
        //            var courseIdsInSchedule = schedule.Select(s => s.course.IDCRS).ToHashSet();
        //            var allCourseIds = sectionsByCourse.Keys.ToHashSet();

        //            return allCourseIds.SetEquals(courseIdsInSchedule);
        //        }

        //        private double CalculatePreferenceMatch(List<Section> schedule, List<int> preferredInstructors)
        //        {
        //            int totalInstructors = schedule.Select(s => s.Instructors).Distinct().Count();
        //            int preferredCount = 0;
        //            foreach (var section in schedule)
        //            {
        //                if(preferredInstructors.Contains(section.Instructors.IdInstructor))
        //                {
        //                    preferredCount++;
        //                }
        //                if (section.Start_Sunday != null && preferredDays["Sunday - Tuesday - Thursday"])
        //                {
        //                    preferredCount++;
        //                }
        //                if (section.Start_Monday != null && preferredDays["Monday - Wednesday"])
        //                {
        //                    preferredCount++;
        //                }
        //            }

        //            return totalInstructors > 0 ? (double)preferredCount / totalInstructors : 0;      
        //        }

        //        private List<List<Section>> SelectSchedules(List<List<Section>> population, List<double> fitnessScores)
        //        {
        //            var selectedSchedules = new List<List<Section>>();
        //            var random = new Random();

        //            // Example selection method: roulette wheel selection
        //            double totalFitness = fitnessScores.Sum();

        //            for (int i = 0; i < population.Count / 2; i++)
        //            {
        //                double rouletteWheelPosition = random.NextDouble() * totalFitness;
        //                double cumulativeFitness = 0;

        //                for (int j = 0; j < population.Count; j++)
        //                {
        //                    cumulativeFitness += fitnessScores[j];
        //                    if (cumulativeFitness >= rouletteWheelPosition)
        //                    {
        //                        selectedSchedules.Add(population[j]);
        //                        break;
        //                    }
        //                }
        //            }

        //            return selectedSchedules;
        //        }

        //        private List<List<Section>> Crossover(List<List<Section>> selectedSchedules)
        //        {
        //            var offspring = new List<List<Section>>();
        //            var random = new Random();

        //            for (int i = 0; i < selectedSchedules.Count / 2; i++)
        //            {
        //                var parent1 = selectedSchedules[i];
        //                var parent2 = selectedSchedules[selectedSchedules.Count - 1 - i];

        //                var child1 = new List<Section>();
        //                var child2 = new List<Section>();

        //                for (int j = 0; j < parent1.Count; j++)
        //                {
        //                    if (random.NextDouble() < 0.5)
        //                    {
        //                        child1.Add(parent1[j]);
        //                        child2.Add(parent2[j]);
        //                    }
        //                    else
        //                    {
        //                        child1.Add(parent2[j]);
        //                        child2.Add(parent1[j]);
        //                    }
        //                }

        //                offspring.Add(child1);
        //                offspring.Add(child2);
        //            }

        //            return offspring;
        //        }

        //        private void Mutate(List<List<Section>> offspring, double mutationRate, Dictionary<int, List<Section>> sectionsByCourse)
        //        {
        //            var random = new Random();

        //            foreach (var schedule in offspring)
        //            {
        //                if (random.NextDouble() < mutationRate)
        //                {
        //                    int index = random.Next(schedule.Count);
        //                    var courseId = schedule[index].course.IDCRS;
        //                    var courseSections = sectionsByCourse[courseId];
        //                    schedule[index] = courseSections[random.Next(courseSections.Count)];
        //                }
        //            }
        //        }

        //        private bool CheckForConflict(List<Section> sections)
        //        {
        //            foreach (var section1 in sections)
        //            {
        //                foreach (var section2 in sections)
        //                {
        //                    if (section1 != section2 && AreSectionsConflicting(section1, section2))
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //            return false;
        //        }

        //        private bool AreSectionsConflicting(Section section1, Section section2)
        //        {
        //            return IsTimeOverlap(section1.Start_Sunday, section1.End_Sunday, section2.Start_Sunday, section2.End_Sunday) ||
        //                   IsTimeOverlap(section1.Start_Monday, section1.End_Monday, section2.Start_Monday, section2.End_Monday) ||
        //                   IsTimeOverlap(section1.Start_Tuesday, section1.End_Tuesday, section2.Start_Tuesday, section2.End_Tuesday) ||
        //                   IsTimeOverlap(section1.Start_Wednesday, section1.End_Wednesday, section2.Start_Wednesday, section2.End_Wednesday) ||
        //                   IsTimeOverlap(section1.Start_Thursday, section1.End_Thursday, section2.Start_Thursday, section2.End_Thursday);
        //        }

        //        private bool IsTimeOverlap(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2)
        //        {
        //            if (start1 == null || end1 == null || start2 == null || end2 == null)
        //                return false;

        //            return start1 < end2 && start2 < end1;
        //        }

        //        public List<List<Section>> CheckForRepetition(List<List<Section>> possibleSchedules)
        //        {
        //            List<string> populationSchedule = new List<string>();

        //            foreach (var schedule in possibleSchedules)
        //            {
        //                string check = "";

        //                foreach (var section in schedule)
        //                {
        //                    check += section.IDSection;
        //                }
        //                if (!populationSchedule.Contains(check))
        //                {
        //                    populationSchedule.Add(check);
        //                    filteredPossibleSchedules.Add(schedule);
        //                }
        //            }
        //            return filteredPossibleSchedules;
        //        }

        //        public async Task<IActionResult> DisplaySchedules()
        //        {
        //            MakeScheduler();
        //            ViewData["possibleSchedules"] = CheckForRepetition(possibleSchedules);
        //            return View();
        //        }
    }
    }
