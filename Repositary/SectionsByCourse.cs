using Microsoft.EntityFrameworkCore;
using Scheduler.Data;
using Scheduler.Models;

namespace Scheduler.Repositary
{
    public class SectionsByCourse
    {
        Dictionary<int, List<Section>> sectionsByCourse;
        public Dictionary<int, List<Section>> FindAllSectionsByCourse(List<Course> selectedCourses, DBContextSystem _context)
        {
            foreach (var course in selectedCourses)
            {
                List<Section> courseSections = new List<Section>();
                var Sections = _context.Sections
                .Where(dc => course.IDCRS == dc.course.IDCRS)
                .Include(dc => dc.Instructors)
                .Include(dc => dc.course)
                .ToList();
                sectionsByCourse.Add(course.IDCRS, Sections);
            }
            return sectionsByCourse;
        }
    }
}


