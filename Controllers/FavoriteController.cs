using Microsoft.AspNetCore.Mvc;
using Kahveci.Models;
using Kahveci.Helpers;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Kahveci.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToFavorite(int productId)
        {
            var userId = HttpContext.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("LogIn", "LoginSignup");
            }

            var existingFavorite = _context.Favorites
                .FirstOrDefault(f => f.CustomerId == userId && f.ProductId == productId);

            if (existingFavorite == null)
            {
                var favorite = new Favorites
                {
                    CustomerId = userId.Value,
                    ProductId = productId
                };
                _context.Favorites.Add(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home"); 
        }

        public bool IsFavorite(int productId)
        {
            var userId = HttpContext.GetUserId();
            if (userId == null) return false;

            var favorite = _context.Favorites
                .FirstOrDefault(f => f.CustomerId == userId && f.ProductId == productId);

            return favorite != null;
        }
        public IActionResult Index()
        {
            var userId = HttpContext.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("LogIn", "LoginSignup");
            }

            var favorites = _context.Favorites
                                    .Where(f => f.CustomerId == userId)
                                    .Include(f => f.Products)
                                    .ToList();
            ViewBag.Favorites = favorites; 
            ViewBag.Categories = _context.ProductCategories.ToList();

            return View(favorites);
        }

        [HttpPost]
        public IActionResult RemoveFromFavorites(int favoriteId)
        {
            var favorite = _context.Favorites.FirstOrDefault(f => f.FavoriteId == favoriteId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
