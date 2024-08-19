using Scheduler.Models;

namespace Scheduler.Services.CalculateFitness
{
    public class InstructorsScore
    {
        public int InstructorScore(Dictionary<List<Section>, int> population,KeyValuePair<List<Section>, int> schedule, List<Instructor> preferredInstructors)
        {
                int score = 0;
                foreach ( var section in schedule.Key)
                {
                   if( preferredInstructors.FirstOrDefault(section.Instructors) != null) score++;
                }
                var maxInstructor = schedule.Key.Count();

            return score;
        }

    }
}
