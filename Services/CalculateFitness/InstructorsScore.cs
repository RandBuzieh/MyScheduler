using Scheduler.Models;

namespace Scheduler.Services.CalculateFitness
{
    public class InstructorsScore
    {
        public Dictionary<List<Section>, int> InstructorScore(Dictionary<List<Section>, int> population, List<Instructor> preferredInstructors)
        {
            foreach (var schedule in population)
            {
                int score = 0;
                foreach ( var section in schedule.Key)
                {
                   if( preferredInstructors.FirstOrDefault(section.Instructors) != null) score++;
                }
                var maxInstructor = schedule.Key.Count();
                population[schedule.Key] += (score * 25 / maxInstructor);
            }
            return population;
        }

    }
}
