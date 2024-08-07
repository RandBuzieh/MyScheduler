using Scheduler.Models;
namespace Scheduler.Services.PopulationGenerating
{
    public interface ICreatePopulation
    {
        public Dictionary<List<Section>, int> InitializePopulation(Dictionary<int, List<Section>> sectionsByCourse, int populationSize);

    }
}
