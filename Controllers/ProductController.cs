using Kahveci.Helpers;
using Kahveci.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kahveci.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ürün Listesi
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.ProductCategories).ToList();
            ViewBag.Categories = _context.ProductCategories.ToList();
            return View(products);
        }

        // Ürün Kategorisine Göre Listeleme
        public IActionResult ByCategory(int id)
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

            ViewBag.Categories = _context.ProductCategories.ToList();
            ViewData["FavoriteProductIds"] = favoriteProductIds;

            var category = _context.ProductCategories
                .FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            var products = _context.Products
                .Include(p => p.ProductCategories)
                .Where(p => p.ProductCategoriesId == id)
                .AsNoTracking()
                .ToList();

            var favorites = _context.Favorites
                .Where(f => f.CustomerId == userId)
                .Include(f => f.Products)
                .AsNoTracking()
                .ToList();

            ViewBag.Favorites = favorites;
            ViewBag.CategoryName = category.CategoryName;

            return View(products);
        }

        // Ürün Ekleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId, ProductName, ProductDescription, Price, ProductType, ImageUrl, ProductCategoriesId")] Products product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.ProductCategories.ToList(); // Kategorileri tekrar yükle
            return View(product);
        }

        // Ürün Güncelleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId, ProductName, ProductDescription, Price, ProductType, ImageUrl, ProductCategoriesId")] Products product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product); // Ürünü güncelle
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.ProductId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.ProductCategories.ToList(); // Kategorileri tekrar yükle
            return View(product);
        }


        // Ürün Silme (GET)
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefault(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Ürün Silme (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
