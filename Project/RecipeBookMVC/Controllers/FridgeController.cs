using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBookMVC.Controllers;

[Authorize] // Холодильник доступний тільки після входу в акаунт
public class FridgeController : Controller
{
    private readonly AppDbContext _context;

    public FridgeController(AppDbContext context)
    {
        _context = context;
    }

    // Гет-метод: відображає сторінку холодильника з доступними категоріями/інгредієнтами
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Отримуємо всі унікальні категорії для фільтрації або відображення
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;

        // Початково показуємо порожній список рецептів, поки користувач нічого не обрав
        return View(Enumerable.Empty<Recipe>());
    }

    // Пост-метод: приймає список обраних інгредієнтів з форми та шукає рецепти
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SearchRecipes(string ingredientsInput)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;

        if (string.IsNullOrWhiteSpace(ingredientsInput))
        {
            ModelState.AddModelError(string.Empty, "Wpisz lub wybierz co najmniej jeden składnik.");
            return View("Index", Enumerable.Empty<Recipe>());
        }

        // Розбиваємо введені користувачем інгредієнти (через кому) та очищаємо пробіли
        var userIngredients = ingredientsInput
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Trim().ToLower())
            .ToList();

        // Завантажуємо всі рецепти з бази даних
        var allRecipes = await _context.Recipes
            .Include(r => r.Category)
            .ToListAsync();

        // Фільтруємо рецепти: залишаємо ті, де інгредієнти з бази містять бодай один інгредієнт користувача
        var matchedRecipes = allRecipes.Where(r =>
            userIngredients.Any(ui => r.Ingredients.ToLower().Contains(ui))
        ).ToList();

        ViewBag.IngredientsQuery = ingredientsInput; // Повертаємо текст назад у поле для зручності

        return View("Index", matchedRecipes);
    }
}