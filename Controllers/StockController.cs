using Microsoft.AspNetCore.Mvc;
using Kahveci.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Kahveci.Controllers
{
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Stok yönetimi sayfası
        public IActionResult Manage()
        {
            // Tüm malzemeleri al
            var ingredients = _context.Ingredients.ToList();
            return View(ingredients); // Malzemeleri View'e gönder
        }

        public IActionResult UpdateStock(int id)
        {
            var ingredient = _context.Ingredients.Find(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        [HttpPost]
        public IActionResult UpdateStock(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                // Aynı isimde başka bir malzeme olup olmadığını kontrol et (id'yi hariç tutarak)
                var existingIngredient = _context.Ingredients
                    .FirstOrDefault(i => i.Name.ToLower() == ingredient.Name.ToLower() && i.IngredientId != ingredient.IngredientId);

                if (existingIngredient != null)
                {
                    // Eğer aynı isimde başka bir malzeme varsa hata mesajı ver
                    ModelState.AddModelError("Name", "Bu isimle zaten bir malzeme bulunmaktadır.");
                    return View(ingredient);
                }

                // Malzeme mevcutsa güncelleme işlemi yapılır
                _context.Ingredients.Update(ingredient); // Update kullanımı
                _context.SaveChanges(); // Veritabanına kaydet
                TempData["SuccessMessage"] = "Stok başarıyla güncellendi!";
                return RedirectToAction(nameof(Manage)); // Stok yönetimine geri dön
            }

            // Eğer model geçerli değilse, güncelleme formunu tekrar göster
            return View(ingredient);
        }


        // Ürün için stok kontrolü ve malzeme ihtiyacını yönetme
        public IActionResult CheckProductStock(int productId)
        {
            var product = _context.Products
                .Include(p => p.ProductIngredients)
                .ThenInclude(pi => pi.Ingredient)
                .FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            var missingIngredients = new List<string>();

            // Ürünün malzeme gereksinimlerini kontrol et
            foreach (var productIngredient in product.ProductIngredients)
            {
                if (productIngredient.Ingredient.StockQuantity < productIngredient.Quantity)
                {
                    missingIngredients.Add($"{productIngredient.Ingredient.Name} - Yetersiz Stok");
                }
            }

            // Eksik malzemeleri listele
            if (missingIngredients.Any())
            {
                return View("MissingIngredients", missingIngredients);
            }

            return View("ProductStockSufficient");
        }

        // Yeni malzeme ekleme sayfası
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                // Aynı malzeme adıyla zaten var mı diye kontrol et
                var existingIngredient = _context.Ingredients
                    .FirstOrDefault(i => i.Name == ingredient.Name); // Aynı isimle var mı?

                if (existingIngredient != null)
                {
                    // Eğer malzeme varsa, sadece stok miktarını güncelle
                    existingIngredient.StockQuantity += ingredient.StockQuantity;
                    _context.Update(existingIngredient);
                    TempData["SuccessMessage"] = "Mevcut malzemenin stok miktarı başarıyla güncellendi!";
                }
                else
                {
                    // Eğer malzeme yoksa, yeni malzeme ekle
                    _context.Ingredients.Add(ingredient);
                    TempData["SuccessMessage"] = "Yeni malzeme başarıyla eklendi!";
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Manage)); // Stok yönetimine geri dön
            }

            return View(ingredient);
        }
    }
}
