using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;

namespace RecipeBookMVC.Controllers;

public class ReviewsController : Controller
{
    private readonly AppDbContext _context;

    public ReviewsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Review review)
    {
        ModelState.Remove("Recipe");

        if (ModelState.IsValid)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Recipes", new { id = review.RecipeId });
        }

        return RedirectToAction("Index", "Recipes");
    }
}