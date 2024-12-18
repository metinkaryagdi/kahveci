using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kahveci.Models;
using Kahveci.Helpers;

namespace Kahveci.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var userId = HttpContext.GetUserId();
            List<int> favoriteProductIds = new List<int>();

            if (userId != null)
            {
                favoriteProductIds = _context.Favorites
                    .Where(f => f.CustomerId == userId)
                    .Select(f => f.ProductId)
                    .ToList();
            }

            var products = _context.Products.Include(p => p.ProductCategories).Take(4).ToList();
            ViewData["FavoriteProductIds"] = favoriteProductIds;
            ViewBag.Categories = _context.ProductCategories.ToList();
            var favorites = _context.Favorites
                                    .Where(f => f.CustomerId == userId)
                                    .Include(f => f.Products)
                                    .ToList();
            ViewBag.Favorites = favorites; 
            return View(products);
        }


    }

}
