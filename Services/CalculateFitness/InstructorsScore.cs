using Scheduler.Models;

namespace Scheduler.Services.CalculateFitness
{
    public class InstructorsScore
    {
        public Dictionary<List<Section>, int> InstructorScore(Dictionary<List<Section>, int> population, List<Instructor> preferredInstructors)
        {
            int score = 0;
            foreach (var schedule in population)
            {
                foreach( var section in schedule.Key)
                {
                    if(preferredInstructors.Contains(section.Instructors)) score++;
                }
                population[schedule.Key] += score;
            }
            return population;
        }

    }
}
