using Scheduler.Models;
namespace Scheduler.Services.CalculateFitness
{
    public class FitnessCheck: IFitnessCheck
    {

        public Dictionary<List<Section>, int> CalculateFitness(Dictionary<List<Section>, int> population, int PreferredStartTime, int PreferredEndTime, Dictionary<string, bool> preferredDays)
        {
            TimeDaysScore _timeDaysScore = new TimeDaysScore();
            _timeDaysScore.CalculateTimeDaysScore(population, PreferredStartTime, PreferredEndTime);

           return population; 
        }

    }
}
