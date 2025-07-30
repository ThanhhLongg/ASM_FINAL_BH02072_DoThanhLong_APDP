using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using MvcAdoDemo.Models;
using Microsoft.Extensions.Configuration;


public class UserController : Controller
{
    private readonly string connectionString = "Server=localhost\\SQLEXPRESS01;Database=EmployeeDB2;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(User user)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Kiểm tra xem username đã tồn tại chưa
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
            checkCmd.Parameters.AddWithValue("@Username", user.Username);

            int userCount = (int)checkCmd.ExecuteScalar();

            if (userCount > 0)
            {
                ViewBag.Error = "Username already exists.";
                return View(); // trả về form đăng ký cùng lỗi
            }

            // Nếu chưa có thì thêm mới
            string query = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Password", user.Password); // Lưu ý: chưa mã hóa password!
            cmd.Parameters.AddWithValue("@Role", user.Role);

            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("Login");
    }


    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(User user)
    {
        if (ModelState.IsValid)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Users WHERE LTRIM(RTRIM(Username)) = LTRIM(RTRIM(@Username)) AND LTRIM(RTRIM(Password)) = LTRIM(RTRIM(@Password))";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    conn.Open();

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string role = reader["Role"].ToString();
                        HttpContext.Session.SetString("username", user.Username);
                        HttpContext.Session.SetString("role", role);

                        if (role == "teacher")
                            return RedirectToAction("Index", "Employee"); // Danh sách sinh viên
                        else
                            return RedirectToAction("Index", "Student"); // Xem thông tin của sinh viên
                    }
                }
            }

            ViewBag.Message = "Invalid login.";
        }

        return View(user);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
