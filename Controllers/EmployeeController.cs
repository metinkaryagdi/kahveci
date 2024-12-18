using Microsoft.AspNetCore.Mvc;
using Kahveci.Models;
using Microsoft.EntityFrameworkCore;

namespace Kahveci.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin veya çalışanlar için genel işlemler sayfası
        public IActionResult Index()
        {
            // Bu sayfada admin paneline yönlendirme veya başka bir işlem yapılabilir
            return View(); 
        }

        // Çalışanları listele - EmployeeList sayfası
        public IActionResult EmployeeList()
        {
            var employees = _context.Employees.ToList(); // Çalışanları veritabanından al
            return View(employees); // Çalışanları EmployeeList view'ına gönder
        }

        // Çalışanları yönetme sayfası - sadece admin için
        public IActionResult Manage()
        {
            // Yönetim için işlemler burada olacak
            return View();
        }

        // Yeni çalışan ekleme sayfası
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(EmployeeList)); // Çalışanlar listesine yönlendir
            }
            return View(employee);
        }

        // Çalışan bilgilerini düzenleme sayfası
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Update(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(EmployeeList)); // Çalışanlar listesine yönlendir
            }
            return View(employee);
        }

        // Çalışan silme sayfası
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction(nameof(EmployeeList)); // Çalışanlar listesine yönlendir
        }
    }
}
