﻿using Scheduler.Models;

namespace Scheduler.Services.CalculateFitness
{
    public class TimeDaysScore
    {
        public Dictionary<List<Section>, int> CalculateTimeDaysScore(Dictionary<List<Section>, int> population,int PreferredStartTime, int PreferredEndTime, Dictionary<string, bool> preferredDays)
        {
            foreach (var schedule in population)
            {
                population[schedule.Key] += CheckForConflict(schedule.Key);
                population[schedule.Key] += CheckForDaysStartEndTime(schedule.Key, PreferredStartTime, PreferredEndTime , preferredDays);
            }

            return population;
        }
        private int CheckForConflict(List<Section> schedule)
        {
            foreach (var section1 in schedule)
            {
                foreach (var section2 in schedule)
                {
                    if (section1 != section2 && AreSectionsConflicting(section1, section2))
                    {
                        return 0;
                    }
                }
            }
            return -100;
        }

        private bool AreSectionsConflicting(Section section1, Section section2)
        {
            return IsTimeOverlap(section1.Start_Sunday, section1.End_Sunday, section2.Start_Sunday, section2.End_Sunday) ||
                   IsTimeOverlap(section1.Start_Monday, section1.End_Monday, section2.Start_Monday, section2.End_Monday) ||
                   IsTimeOverlap(section1.Start_Tuesday, section1.End_Tuesday, section2.Start_Tuesday, section2.End_Tuesday) ||
                   IsTimeOverlap(section1.Start_Wednesday, section1.End_Wednesday, section2.Start_Wednesday, section2.End_Wednesday) ||
                   IsTimeOverlap(section1.Start_Thursday, section1.End_Thursday, section2.Start_Thursday, section2.End_Thursday);
        }

        private bool IsTimeOverlap(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2)
        {
            if (start1 == null || end1 == null || start2 == null || end2 == null)
                return false;

            return start1 < end2 && start2 < end1;
        }

        private int CheckForDaysStartEndTime(List<Section> schedule, int PreferredStartTime,int PreferredEndTime,Dictionary<string, bool> preferredDays)
        {
            int score = 0;

            foreach (var section in schedule)
            {
                if(section.Start_Sunday.Value.Hour >= PreferredStartTime || section.Start_Monday.Value.Hour >= PreferredStartTime || section.Start_Tuesday.Value.Hour >= PreferredStartTime
                 ||  section.Start_Wednesday.Value.Hour >= PreferredStartTime || section.Start_Thursday.Value.Hour >= PreferredStartTime ) score++;

                if(section.End_Sunday.Value.Hour <= PreferredEndTime || section.End_Monday.Value.Hour <= PreferredEndTime || section.End_Tuesday.Value.Hour <= PreferredEndTime
                        || section.End_Wednesday.Value.Hour <= PreferredEndTime || section.End_Thursday.Value.Hour <= PreferredEndTime) score++;

                if (section.Start_Sunday != null && preferredDays["Sunday"]) score++;
                if (section.Start_Monday != null && preferredDays["Monday"]) score++;
                if (section.Start_Tuesday != null && preferredDays["Tuesday"]) score++;
                if (section.Start_Wednesday != null && preferredDays["Wednesday"]) score++;
                if (section.Start_Thursday != null && preferredDays["Thursday"]) score++;
            }
            return score * 75 / schedule.Capacity;
        }


    }
}
