using System.Collections.Generic;
using System.Linq;

namespace ResumeAnalyzer.Services
{
    public class MatchingService
    {
        public static int Calculate(List<string> resumeSkills, string jobSkills)
        {
            if (string.IsNullOrEmpty(jobSkills))
                return 0;

            var jobList = jobSkills.ToLower()
                                   .Split(',')
                                   .Select(x => x.Trim())
                                   .ToList();

            int match = 0;

            foreach (var skill in jobList)
            {
                if (resumeSkills.Contains(skill))
                    match++;
            }

            if (jobList.Count == 0)
                return 0;

            return (match * 100) / jobList.Count;
        }
    }
}