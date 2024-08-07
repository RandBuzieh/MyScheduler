using Scheduler.Models;
namespace Scheduler.Services.CalculateFitness
{
    public interface IFitnessCheck
    {
        public Dictionary<List<Section>, int> CalculateFitness(Dictionary<List<Section>, int> population, int PreferredStartTime, int PreferredEndTime, Dictionary<string, bool> preferredDays);

    }
}
