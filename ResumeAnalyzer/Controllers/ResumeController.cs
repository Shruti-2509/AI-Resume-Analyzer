using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using ResumeAnalyzer.Data;
using ResumeAnalyzer.Models;
using ResumeAnalyzer.Utils;
using ResumeAnalyzer.Services;
using System.IO;
using System.Linq;

namespace ResumeAnalyzer.Controllers
{
    public class ResumeController : Controller
    {
        // GET: /Resume/Upload
        public IActionResult Upload()
        {
            if (HttpContext.Session.GetInt32("user_id") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: /Resume/Upload
        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Msg = "Please select a file.";
                return View();
            }

            // 1) Save file
            string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // 2) Extract text
            string text = "";
            string ext = Path.GetExtension(file.FileName).ToLower();

            if (ext == ".pdf")
                text = PdfParser.ExtractText(filePath);
            else if (ext == ".docx")
                text = DocParser.ExtractText(filePath);
            else
            {
                ViewBag.Msg = "Only PDF and DOCX allowed.";
                return View();
            }

            // 3) Extract skills
            var resumeSkills = SkillService.ExtractSkills(text);

            int userId = HttpContext.Session.GetInt32("user_id") ?? 0;

            // 4) Save resume
            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                string q = "INSERT INTO Resumes(user_id,file_path,extracted_text) VALUES(@u,@p,@t)";
                var cmd = new MySqlCommand(q, con);

                cmd.Parameters.AddWithValue("@u", userId);
                cmd.Parameters.AddWithValue("@p", fileName);
                cmd.Parameters.AddWithValue("@t", text);

                cmd.ExecuteNonQuery();
            }

            // 5) Matching
            var results = new List<Tuple<string, int>>();

            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                var cmd = new MySqlCommand("SELECT * FROM Jobs", con);
                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    string jobTitle = r["job_title"]?.ToString() ?? "";
                    string jobSkills = r["required_skills"]?.ToString() ?? "";

                    int score = MatchingService.Calculate(resumeSkills, jobSkills);
                    results.Add(Tuple.Create(jobTitle, score));
                }
            }

            // 6) Sort
            results = results.OrderByDescending(x => x.Item2).ToList();

            // 7) Best job + score
            var bestJob = results.FirstOrDefault();
            int resumeScore = bestJob != null ? bestJob.Item2 : 0;

            // 🔥 8) SAVE RESULT TO DATABASE (IMPORTANT FIX)
            if (bestJob != null)
            {
                using (var con = DbConnection.GetConnection())
                {
                    con.Open();

                    // Remove old result for same user (avoid duplicates)
                    string deleteOld = "DELETE FROM Results WHERE user_id=@u";
                    var delCmd = new MySqlCommand(deleteOld, con);
                    delCmd.Parameters.AddWithValue("@u", userId);
                    delCmd.ExecuteNonQuery();

                    // Insert new result
                    string insert = @"INSERT INTO Results(user_id, match_percentage) 
                  VALUES(@u, @m)";
                    var cmd = new MySqlCommand(insert, con);
                    cmd.Parameters.AddWithValue("@u", userId);
                    cmd.Parameters.AddWithValue("@m", resumeScore);

                    cmd.ExecuteNonQuery();
                }
            }

            // 9) Suggestions
            List<string> suggestions = new List<string>();

            if (bestJob != null)
            {
                string bestSkills = "";

                using (var con = DbConnection.GetConnection())
                {
                    con.Open();

                    var cmd = new MySqlCommand("SELECT required_skills FROM Jobs WHERE job_title=@t", con);
                    cmd.Parameters.AddWithValue("@t", bestJob.Item1);

                    bestSkills = cmd.ExecuteScalar()?.ToString() ?? "";
                }

                var jobList = bestSkills.ToLower().Split(',').Select(x => x.Trim()).ToList();

                foreach (var s in jobList)
                {
                    if (!resumeSkills.Contains(s))
                        suggestions.Add(s);
                }
            }

            // 10) Send to view
            ViewBag.Results = results;
            ViewBag.Skills = resumeSkills;
            ViewBag.Score = resumeScore;
            ViewBag.BestJob = bestJob;
            ViewBag.Suggestions = suggestions;

            return View("Result");
        }
    }
}