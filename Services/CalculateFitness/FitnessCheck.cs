using Scheduler.Models;
namespace Scheduler.Services.CalculateFitness
{
    public class FitnessCheck: IFitnessCheck
    {

        public Dictionary<List<Section>, int> CalculateFitness(Dictionary<List<Section>, int> population, int PreferredStartTime, int PreferredEndTime, Dictionary<string, bool> preferredDays, List<Instructor> preferredInstructors)
        {
            TimeDaysScore _timeDaysScore = new TimeDaysScore();
            InstructorsScore _instructorScore = new InstructorsScore();
            population = _timeDaysScore.CalculateTimeDaysScore(population, PreferredStartTime, PreferredEndTime, preferredDays);
            population = _instructorScore.InstructorScore(population, preferredInstructors);
           return population; 
        }

    }
}
