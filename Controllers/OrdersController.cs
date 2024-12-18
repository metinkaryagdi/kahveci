using Kahveci.Helpers;
using Kahveci.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kahveci.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Siparişleri listele
        public IActionResult Index()
        {

            return View();
        }
    }
}
