using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ResumeAnalyzer.Data;
using ResumeAnalyzer.Models;

namespace ResumeAnalyzer.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please enter email and password.";
                return View();
            }

            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                string q = "SELECT * FROM Users WHERE email=@e AND password=@p";
                var cmd = new MySqlCommand(q, con);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@p", password);

                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        HttpContext.Session.SetInt32("user_id", Convert.ToInt32(r["user_id"]));
                        HttpContext.Session.SetString("name", r["name"].ToString());
                        HttpContext.Session.SetString("role", r["role"].ToString());

                        // Role-based redirect
                        if (r["role"].ToString() == "admin")
                            return RedirectToAction("Dashboard", "Admin");

                        return RedirectToAction("Upload", "Resume");
                    }
                }
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (string.IsNullOrWhiteSpace(user.name) ||
                string.IsNullOrWhiteSpace(user.email) ||
                string.IsNullOrWhiteSpace(user.password))
            {
                ViewBag.Msg = "All fields are required.";
                return View();
            }

            using (var con = DbConnection.GetConnection())
            {
                con.Open();

                // Check if email exists
                string check = "SELECT COUNT(*) FROM Users WHERE email=@e";
                var checkCmd = new MySqlCommand(check, con);
                checkCmd.Parameters.AddWithValue("@e", user.email);

                var exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (exists > 0)
                {
                    ViewBag.Msg = "Email already registered.";
                    return View();
                }

                // Insert user (default role = user)
                string q = "INSERT INTO Users(name,email,password,role) VALUES(@n,@e,@p,'user')";
                var cmd = new MySqlCommand(q, con);

                cmd.Parameters.AddWithValue("@n", user.name);
                cmd.Parameters.AddWithValue("@e", user.email);
                cmd.Parameters.AddWithValue("@p", user.password);

                cmd.ExecuteNonQuery();
            }

            ViewBag.Msg = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}