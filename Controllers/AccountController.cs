// using Microsoft.AspNetCore.Mvc;
// using MvcAdoDemo.Models;
// using System.Linq;
// using Microsoft.AspNetCore.Http;

// namespace MvcAdoDemo.Controllers
// {
//     public class AccountController : Controller
//     {
//         private readonly AppDbContext _context;

//         public AccountController(AppDbContext context)
//         {
//             _context = context;
//         }

//         [HttpGet]
//         public IActionResult Login()
//         {
//             return View();
//         }

//         [HttpPost]
//         public IActionResult Login(string username, string password)
//         {
//             var user = _context.Users
//                 .FirstOrDefault(u => u.Username == username && u.Password == password);

//             if (user != null)
//             {
//                 HttpContext.Session.SetString("Username", user.Username);
//                 HttpContext.Session.SetString("Role", user.Role);

//                 if (user.Role == "admin")
//                     return RedirectToAction("Index", "Admin");
//                 else
//                     return RedirectToAction("Index", "Home");
//             }

//             ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu.";
//             return View();
//         }

//         public IActionResult Logout()
//         {
//             HttpContext.Session.Clear();
//             return RedirectToAction("Login");
//         }
//     }
// }
