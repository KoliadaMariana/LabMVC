using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;

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

    // GET: Recipes
    public async Task<IActionResult> Index(string searchString, string category)
    {
        var recipesQuery = _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            recipesQuery = recipesQuery.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
        }

        if (!string.IsNullOrEmpty(category))
        {
            recipesQuery = recipesQuery.Where(x => x.Category.Name == category);
        }

        ViewData["Categories"] = await _context.Categories.Select(c => c.Name).ToListAsync();
        return View(await recipesQuery.ToListAsync());
    }

    // GET: Recipes/Create
    [Authorize]
    public async Task<IActionResult> Create()
    {
        ViewData["CategoryId"] = new SelectList(await _context.Set<Category>().ToListAsync(), "Id", "Name");
        return View();
    }

    // POST: Recipes/Create
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
            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoryId"] = new SelectList(await _context.Set<Category>().ToListAsync(), "Id", "Name", recipe.CategoryId);
        return View(recipe);
    }

    // GET: Recipes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var recipe = await _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.User)
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (recipe == null) return NotFound();

        return View(recipe);
    }

    // --- НОВІ МЕТОДИ ДЛЯ БЕЗПЕЧНОГО ВИDАЛЕННЯ ВЛАСНИКОМ ---

    // GET: Recipes/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var recipe = await _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (recipe == null) return NotFound();

        // Перевіряємо, чи є поточний користувач власником цього рецепту
        var currentUserId = _userManager.GetUserId(User);
        if (recipe.UserId != currentUserId)
        {
            return Forbid(); // Якщо чужий рецепт — показуємо помилку доступу (Access Denied)
        }

        return View(recipe);
    }

    // POST: Recipes/Delete/5
    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null) return NotFound();

        // Захисна перевірка безпосередньо перед видаленням з бази даних
        var currentUserId = _userManager.GetUserId(User);
        if (recipe.UserId != currentUserId)
        {
            return Forbid();
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}