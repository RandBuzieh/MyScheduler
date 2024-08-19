using Microsoft.EntityFrameworkCore;
using Scheduler.Data;
using Scheduler.Models;

namespace Scheduler.Repositary
{
    public class StudentInfo
    {
        public List<int> FinishedCourses(Student _student, DBContextSystem _context)
        {
            // Fetch completed course IDs for the _student
            var completedCourseIds = _context.StudentsProgress
                .Where(p => p.Student.KeyStudent == _student.KeyStudent)
                .Select(p => p.course.IDCRS)
                .ToList();

            return completedCourseIds;
        }
      
        public List<Course> GetAvailableCoursesAsync(Student _student, DBContextSystem _context)
        {
            var completedCourseIds = FinishedCourses(_student, _context);

            var availableCourses = _context.DegreeProgresContents
                .Where(dc => !completedCourseIds.Contains(dc.course.IDCRS))
                .Select(dc => dc.course)
                .Distinct()
                .ToList();

            // Remove courses that need a prerequisite
            var coursesStudentCanTake = new List<Course>();
            foreach (var course in availableCourses)
            {
                var prerequisiteIds = _context.PlanContents
                    .Where(pc => pc.course.IDCRS == course.IDCRS)
                    .Where(pc => pc.prerequisite != 0)
                    .Select(pc => pc.prerequisite)
                    .ToList();

                if (!prerequisiteIds.Any() || prerequisiteIds.All(prerequisite => completedCourseIds.Contains((int)prerequisite)))
                {
                    coursesStudentCanTake.Add(course);
                }
            }

            var studyPlanId = _context.Students
                .Where(s => s.KeyStudent == _student.KeyStudent)
                .Select(s => s.studyPlan.IdStudyPlan)
                .FirstOrDefault();

            // Fetch courses from the _student's study plan and ensure they have sections
            var studyPlanCourses = _context.PlanContents
                .Where(pc => pc.StudyPlan.IdStudyPlan == studyPlanId)
                .Where(pc => coursesStudentCanTake.Contains(pc.course))
                .Where(pc => _context.Sections.Any(s => s.course.IDCRS == pc.course.IDCRS))
                .Select(pc => pc.course)
                .Distinct()
                .ToList();

            return studyPlanCourses;
        }

    }
}
