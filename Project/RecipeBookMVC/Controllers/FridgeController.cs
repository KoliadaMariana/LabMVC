using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;

namespace RecipeBookMVC.Controllers
{
    [Authorize]
    public class FridgeController : Controller
    {
        private readonly AppDbContext _context;

        public FridgeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(List<string>? selectedIngredients)
        {
            var recipes = await _context.Recipes
                .Include(r => r.Category)
                .ToListAsync();

            var availableIngredients = recipes
                .Where(r => !string.IsNullOrEmpty(r.Ingredients))
                .SelectMany(r => r.Ingredients.Split(','))
                .Select(i => i.Trim().ToLower())
                .Where(i => !string.IsNullOrEmpty(i))
                .Distinct()
                .OrderBy(i => i)
                .ToList();

            ViewBag.AvailableIngredients = availableIngredients;
            ViewBag.SelectedIngredients = selectedIngredients ?? new List<string>();

            if (selectedIngredients == null || !selectedIngredients.Any())
            {
                return View(recipes);
            }

            var matchedRecipes = recipes.Where(r =>
                selectedIngredients.All(i =>
                    r.Ingredients != null &&
                    r.Ingredients.ToLower().Contains(i.ToLower())))
                .ToList();

            return View(matchedRecipes);
        }
    }
}