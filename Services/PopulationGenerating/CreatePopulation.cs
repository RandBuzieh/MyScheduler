using Scheduler.Data;
using Scheduler.Models;
using Scheduler.Repositary;
namespace Scheduler.Services.PopulationGenerating
{
    public class CreatePopulation : ICreatePopulation
    {

        public List<List<Section>> InitializePopulation(Dictionary<int, List<Section>> sectionsByCourse, int populationSize)
        {
            var population = new List<List<Section>>();
            var random = new Random();
            for (int i = 0; i < populationSize; i++)
            {
                var schedule = new List<Section>();
                foreach (var courseSections in sectionsByCourse.Values)
                {
                    Section section = courseSections[random.Next(courseSections.Count)];
                    schedule.Add(section);
                }

                population.Add(schedule);
            }
            return population;
      
        }
    }
}
