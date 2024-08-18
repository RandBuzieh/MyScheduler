using Scheduler.Models;
namespace Scheduler.Services.CalculateFitness
{
    public class FitnessCheck: IFitnessCheck
    {

        public Dictionary<List<Section>, int> CalculateFitness(Dictionary<List<Section>, int> population, int PreferredStartTime, int PreferredEndTime, Dictionary<string, bool> preferredDays, List<Instructor> preferredInstructors)
        {
            TimeDaysScore _timeDaysScore = new TimeDaysScore();
            InstructorsScore _instructorScore = new InstructorsScore();
            foreach (var schedule in population)
            {
                population[schedule.Key] = _timeDaysScore.CalculateTimeDaysScore(population,schedule, PreferredStartTime, PreferredEndTime, preferredDays);
                if(schedule.Value>0) population[schedule.Key] += _instructorScore.InstructorScore(population,schedule, preferredInstructors);
            }
           return population; 
        }

    }
}
