using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RecipeBookMVC.Controllers;

public class RecipesController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecipesController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string searchString, string category, int? maxTime)
    {
        var allRecipes = await _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.User)
            .ToListAsync();

        var matchedRecipes = new List<Recipe>();

        foreach (var r in allRecipes)
        {
            bool matchesSearch = true;
            bool matchesCategory = true;
            bool matchesTime = true;

            if (!string.IsNullOrEmpty(searchString))
            {
                if (r.Name == null || !r.Name.ToLower().Contains(searchString.ToLower()))
                {
                    matchesSearch = false;
                }
            }

            if (!string.IsNullOrEmpty(category))
            {
                if (r.Category == null || r.Category.Name != category)
                {
                    matchesCategory = false;
                }
            }

            if (maxTime.HasValue)
            {
                if (r.CookingTime > maxTime.Value)
                {
                    matchesTime = false;
                }
            }

            if (matchesSearch && matchesCategory && matchesTime)
            {
                matchedRecipes.Add(r);
            }
        }

        var categoriesList = await _context.Categories.ToListAsync();
        var categoryNames = new List<string>();
        foreach (var c in categoriesList)
        {
            categoryNames.Add(c.Name);
        }

        ViewData["Categories"] = categoryNames;
        ViewBag.SelectedCategory = category;
        ViewBag.SearchString = searchString;
        ViewBag.MaxTime = maxTime;

        return View(matchedRecipes);
    }
    [Authorize]
    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Recipe recipe)
    {
        ModelState.Remove("UserId");
        ModelState.Remove("User");
        ModelState.Remove("Category");

        if (ModelState.IsValid)
        {
            recipe.UserId = _userManager.GetUserId(User);
            _context.Add(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        var categories = await _context.Categories.ToListAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", recipe.CategoryId);
        return View(recipe);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var recipe = await _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.User)
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (recipe == null)
        {
            return NotFound();
        }

        return View(recipe);
    }

    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var recipe = await _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (recipe == null)
        {
            return NotFound();
        }

        var currentUserId = _userManager.GetUserId(User);
        if (recipe.UserId != currentUserId)
        {
            return Forbid();
        }

        return View(recipe);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }

        var currentUserId = _userManager.GetUserId(User);
        if (recipe.UserId != currentUserId)
        {
            return Forbid();
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}