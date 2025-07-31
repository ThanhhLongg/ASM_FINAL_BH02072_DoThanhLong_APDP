using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MvcAdoDemo.Models;
using Microsoft.Data.SqlClient;

namespace MvcAdoDemo.Controllers
{
        public class StudentController : Controller
        {
             public IActionResult Index()
                {
                    var role = HttpContext.Session.GetString("role");

                    if (string.IsNullOrEmpty(role))
                    {
                        // Nếu chưa đăng nhập, quay lại trang login
                        return RedirectToAction("Login", "User");
                    }

                    if (role.ToLower() != "student")
                    {
                        // Nếu không phải role student thì không cho vào trang này
                        return RedirectToAction("Login", "User");
                    }

                    // Nếu hợp lệ thì hiển thị Index
                    return View();
                }   


        public IActionResult Info()
        {
            var username = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            string connectionString = "Server=localhost\\SQLEXPRESS01;Database=EmployeeDB2;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
            User student = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Account WHERE Username = @Username AND Role = 'student'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        student = new User
                        {
                            Username = reader["Username"].ToString(),
                            Role = reader["Role"].ToString(),
                            StudentId = reader["StudentId"]?.ToString(),
                            Name = reader["Name"]?.ToString(),
                            Gender = reader["Gender"]?.ToString(),
                            City = reader["City"]?.ToString()
                        };
                    }
                }
            }

            if (student == null)
            {
                ViewBag.Error = "Không tìm thấy thông tin sinh viên.";
                return View(); // Trả về view rỗng nhưng có thông báo
            }

            return View(student);
        }
    }
}
