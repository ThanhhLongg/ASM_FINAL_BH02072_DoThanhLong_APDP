using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MvcAdoDemo.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public IActionResult RegisterStudent(string username)
        {
            ViewBag.Username = username;
            return View();
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("role") != "student")
                return RedirectToAction("Login", "User");

            // TODO: Lấy dữ liệu sinh viên cần thiết từ DB nếu muốn
            return View();
        }

        public IActionResult Info()
        {
            string username = HttpContext.Session.GetString("username");

            // TODO: Lấy chi tiết thông tin sinh viên từ DB theo username
            return View();
        }
        
    }
}
