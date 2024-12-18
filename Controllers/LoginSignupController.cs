using System.Diagnostics;
using Kahveci.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Kahveci.Controllers
{
    public class LoginSignupController : Controller
    {
        private readonly ILogger<LoginSignupController> _logger;
        private readonly ApplicationDbContext _context;

        public LoginSignupController(ILogger<LoginSignupController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Anasayfa ya da giriş sayfası
        public IActionResult Index()
        {
            return View();
        }

        // Müşteri Giriş Sayfası (GET)
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        // Müşteri Giriş İşlemi (POST)
        [HttpPost]
        public IActionResult LogIn(string CustomerEmail, string Password)
        {
            var customer = _context.Customers
                                   .FirstOrDefault(c => c.CustomerEmail == CustomerEmail && c.Password == Password);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("UserId", customer.CustomerId);
                return RedirectToAction("Index", "Home"); // Müşteriyi ana sayfaya yönlendir
            }

            ViewBag.ErrorMessage = "Email or Password is incorrect.";
            return View();
        }

        // Çalışan Giriş Sayfası (GET)
        [HttpGet]
        public IActionResult EmployeeLogIn()
        {
            return View();
        }

        // Çalışan Giriş İşlemi (POST)
        [HttpPost]
        public IActionResult EmployeeLogIn(string EmployeeEmail, string Password)
        {
            var employee = _context.Employees
                                   .FirstOrDefault(e => e.EmployeeEmail == EmployeeEmail && e.Password == Password);

            if (employee != null)
            {
                HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId); // Çalışan ID'sini session'a kaydediyoruz
                return RedirectToAction("Index", "Employee"); // Çalışan başarılı giriş yaptıysa yönetim paneline yönlendir
            }

            ViewBag.ErrorMessage = "Email or Password is incorrect.";
            return View();
        }

        // Müşteri Kayıt Sayfası (GET)
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        // Müşteri Kayıt İşlemi (POST)
        [HttpPost]
        public IActionResult SignUp(Customer customer, string ConfirmPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            if (customer.Password != ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View(customer);
            }

            if (_context.Customers.Any(c => c.CustomerEmail == customer.CustomerEmail))
            {
                ViewBag.ErrorMessage = "This email is already registered.";
                return View(customer);
            }

            _context.Customers.Add(customer);
            _context.SaveChanges();

            TempData["Message"] = "Registration successful! Please log in.";
            return RedirectToAction("LogIn");
        }

        // Çıkış işlemi
        public IActionResult LogOut()
        {
            // Müşteri için session'ı sil
            HttpContext.Session.Remove("UserId");

            // Çalışan için session'ı sil
            HttpContext.Session.Remove("EmployeeId");

            return RedirectToAction("Index", "LoginSignup");
        }
    }
}
