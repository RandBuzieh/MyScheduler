using Scheduler.Models;
namespace Scheduler.Services.PopulationGenerating
{
    public interface ICreatePopulation
    {
        public List<List<Section>> InitializePopulation(Dictionary<int, List<Section>> sectionsByCourse, int populationSize);

    }
}
