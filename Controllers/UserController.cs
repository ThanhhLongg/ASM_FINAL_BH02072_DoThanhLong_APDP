using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MvcAdoDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System;
using Microsoft.Extensions.Configuration;

namespace MvcAdoDemo.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: User/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        public IActionResult Register(Account user)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra xem Username đã tồn tại chưa
                    string checkSql = "SELECT COUNT(*) FROM Account WHERE Username = @Username";
                    SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@Username", user.Username);
                    int userCount = (int)checkCmd.ExecuteScalar();

                    if (userCount > 0)
                    {
                        ModelState.AddModelError("Username", "Username already exists.");
                        return View(user);
                    }

                    // Nếu chưa tồn tại, tiến hành insert
                    string sql = "INSERT INTO Account (Username, Password, Role, StudentId, Name, Gender, City) " +
                                "VALUES (@Username, @Password, @Role, @StudentId, @Name, @Gender, @City)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role ?? "student");
                    cmd.Parameters.AddWithValue("@StudentId", user.StudentId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", user.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@City", user.City ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToAction("Login");
            }

            return View(user);
        }


        // GET: User/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public IActionResult Login(Account user)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM Account WHERE Username = @Username AND Password = @Password";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        HttpContext.Session.SetString("username", reader["Username"].ToString());
                        HttpContext.Session.SetString("role", reader["Role"].ToString());

                        string role = reader["Role"].ToString().ToLower();
                        if (role == "admin")
                        {
                            return RedirectToAction("Index", "Account");
                        }
                        else if (role == "student")
                        {
                            return RedirectToAction("Index", "Student");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Unknown role.";
                            return View(user);
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid username or password.";
                        return View(user);
                    }
                }
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
