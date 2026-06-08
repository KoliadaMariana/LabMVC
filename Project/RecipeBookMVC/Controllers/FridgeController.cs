using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;

namespace RecipeBookMVC.Controllers
{
    public class FridgeController : Controller
    {
        private readonly AppDbContext _context;

        public FridgeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(List<string> selectedIngredients)
        {
            var allRecipes = await _context.Recipes.ToListAsync();
            var availableIngredients = new List<string>();

            foreach (var r in allRecipes)
            {
                if (r.Ingredients != null)
                {
                    var parts = r.Ingredients.Split(',');
                    foreach (var p in parts)
                    {
                        var cleanPart = p.Trim().ToLower();
                        if (!availableIngredients.Contains(cleanPart) && cleanPart != "")
                        {
                            availableIngredients.Add(cleanPart);
                        }
                    }
                }
            }

            ViewBag.AvailableIngredients = availableIngredients;
            ViewBag.SelectedIngredients = selectedIngredients;

            var matchedRecipes = new List<Recipe>();

            if (selectedIngredients != null && selectedIngredients.Count > 0)
            {
                foreach (var r in allRecipes)
                {
                    if (r.Ingredients != null)
                    {
                        bool hasAll = true;
                        foreach (var needed in selectedIngredients)
                        {
                            if (!r.Ingredients.ToLower().Contains(needed.ToLower()))
                            {
                                hasAll = false;
                                break;
                            }
                        }
                        if (hasAll)
                        {
                            matchedRecipes.Add(r);
                        }
                    }
                }
            }
            else
            {
                matchedRecipes = allRecipes;
            }

            return View(matchedRecipes);
        }
    }
}