using System.Collections.Generic;

namespace ResumeAnalyzer.Services
{
    public class SkillService
    {
        public static List<string> ExtractSkills(string text)
        {
            var skills = new List<string>
            {
                "c#", "asp.net", "mvc", "sql",
                "java", "python", "html", "css",
                "javascript", "bootstrap"
            };

            var found = new List<string>();

            if (string.IsNullOrEmpty(text))
                return found;

            text = text.ToLower();

            foreach (var skill in skills)
            {
                if (text.Contains(skill))
                    found.Add(skill);
            }

            return found;
        }
    }
}