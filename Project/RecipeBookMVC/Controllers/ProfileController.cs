using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBookMVC.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var recipes = await _context.Recipes
                .Include(r => r.Reviews)
                .Where(r => r.UserId == user.Id)
                .ToListAsync();

            ViewBag.RecipesCount = recipes.Count;

            double averageRating = 0;

            var allReviews = recipes
                .SelectMany(r => r.Reviews)
                .ToList();

            if (allReviews.Any())
            {
                averageRating = allReviews.Average(r => r.Rating);
            }

            ViewBag.AverageRating = averageRating;
            ViewBag.IsPremium = user.EmailConfirmed;

            return View(recipes);
        }
    }
}