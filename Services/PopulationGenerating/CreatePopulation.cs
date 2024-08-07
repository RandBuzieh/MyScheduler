using Scheduler.Models;
namespace Scheduler.Services.PopulationGenerating
{
    public class CreatePopulation : ICreatePopulation
    {
        public Dictionary<List<Section>, int> InitializePopulation(Dictionary<int, List<Section>> sectionsByCourse, int populationSize)
        {
        HashSet<int> populationIndex = new HashSet<int>();
        var population = new Dictionary<List<Section>,int>();
            var random = new Random();
            for (int i = 0; i < populationSize; i++)
            {
                string index = "";
                var schedule = new List<Section>();
                foreach (var courseSections in sectionsByCourse.Values)
                {
                    Section section = courseSections[random.Next(courseSections.Count)];
                    index += section.IDSection.ToString();
                    schedule.Add(section);
                }
                if(!populationIndex.Contains(int.Parse(index)))
                {
                    populationIndex.Add(int.Parse(index));
                    population[schedule] = 0;
                }
            }
            return population;
      
        }
    }
}
