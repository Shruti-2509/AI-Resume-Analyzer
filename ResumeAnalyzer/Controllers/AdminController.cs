using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ResumeAnalyzer.Data;
using ResumeAnalyzer.Models;

namespace ResumeAnalyzer.Controllers
{
    public class AdminController : Controller
    {
        // ================= DASHBOARD =================
        public IActionResult Dashboard()
        {
            int users = 0, jobs = 0, resumes = 0;

            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                var cmd1 = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE role!='admin'", con);
                users = Convert.ToInt32(cmd1.ExecuteScalar());

                var cmd2 = new MySqlCommand("SELECT COUNT(*) FROM Jobs", con);
                jobs = Convert.ToInt32(cmd2.ExecuteScalar());

                var cmd3 = new MySqlCommand("SELECT COUNT(*) FROM Resumes", con);
                resumes = Convert.ToInt32(cmd3.ExecuteScalar());
            }

            ViewBag.Users = users;
            ViewBag.Jobs = jobs;
            ViewBag.Resumes = resumes;

            return View();
        }

        // ================= MANAGE JOBS =================
        public IActionResult ManageJobs(string search, string category)
        {
            var list = new List<Job>();

            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                string query = "SELECT * FROM Jobs WHERE 1=1";

                if (!string.IsNullOrEmpty(search))
                    query += " AND job_title LIKE @s";

                if (!string.IsNullOrEmpty(category))
                    query += " AND category=@c";

                var cmd = new MySqlCommand(query, con);

                if (!string.IsNullOrEmpty(search))
                    cmd.Parameters.AddWithValue("@s", "%" + search + "%");

                if (!string.IsNullOrEmpty(category))
                    cmd.Parameters.AddWithValue("@c", category);

                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new Job
                    {
                        job_id = (int)r["job_id"],
                        job_title = r["job_title"].ToString(),
                        category = r["category"].ToString(),
                        required_skills = r["required_skills"].ToString()
                    });
                }
            }

            return View(list);
        }

        // INSERT
        [HttpPost]
        public IActionResult AddJobManage(Job job)
        {
            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                string q = "INSERT INTO Jobs(job_title,category,required_skills) VALUES(@t,@c,@s)";
                var cmd = new MySqlCommand(q, con);

                cmd.Parameters.AddWithValue("@t", job.job_title);
                cmd.Parameters.AddWithValue("@c", job.category);
                cmd.Parameters.AddWithValue("@s", job.required_skills);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("ManageJobs");
        }

        // DELETE JOB
        public IActionResult DeleteJob(int id)
        {
            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                var cmd = new MySqlCommand("DELETE FROM Jobs WHERE job_id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("ManageJobs");
        }

        // ================= MANAGE CANDIDATES =================
        public IActionResult ManageCandidates()
        {
            var users = new List<User>();

            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                var cmd = new MySqlCommand("SELECT * FROM Users WHERE role!='admin'", con);
                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    users.Add(new User
                    {
                        user_id = (int)r["user_id"],
                        name = r["name"].ToString(),
                        email = r["email"].ToString(),
                        role = r["role"].ToString()
                    });
                }
            }

            return View(users);
        }

        // DELETE USER
        public IActionResult DeleteUser(int id)
        {
            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                var cmd = new MySqlCommand("DELETE FROM Users WHERE user_id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("ManageCandidates");
        }

        // ================= VIEW CANDIDATE =================
        public IActionResult ViewCandidate(int id)
        {
            var list = new List<dynamic>();

            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                string q = @"
        SELECT r.resume_id, r.file_path, u.name
        FROM Resumes r
        JOIN Users u ON r.user_id = u.user_id
        WHERE r.user_id=@id";

                var cmd = new MySqlCommand(q, con);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new
                    {
                        resume_id = reader["resume_id"],
                        file_path = reader["file_path"].ToString(),
                        name = reader["name"].ToString()
                    });
                }
            }

            ViewBag.UserId = id;
            return View(list);
        }

        public IActionResult DeleteResume(int id)
        {
            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                var cmd = new MySqlCommand("DELETE FROM Resumes WHERE resume_id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("ViewCandidate", new { id = Request.Query["userId"] });
        }



        // ================= TOP CANDIDATES =================
        public IActionResult TopCandidates()
        {
            var list = new List<dynamic>();

            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                string q = @"
        SELECT u.name, MAX(r.match_percentage) AS score
        FROM Results r
        JOIN Users u ON r.user_id = u.user_id
        GROUP BY r.user_id
        HAVING score >= 75
        ORDER BY score DESC";

                var cmd = new MySqlCommand(q, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new
                    {
                        name = reader["name"].ToString(),
                        score = reader["score"].ToString()
                    });
                }
            }

            return View(list);
        }
    }
}