using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;

namespace RecipeBookMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var stats = new DashboardViewModel();

            // 1. Кількість рецептів
            stats.TotalRecipes = await _context.Recipes.CountAsync();

            // Safe-check для відгуків (якщо таблиця вже існує)
            try
            {
                stats.TotalReviews = await _context.Set<Review>().CountAsync();
                if (stats.TotalReviews > 0)
                {
                    stats.AverageRating = Math.Round(await _context.Reviews.AverageAsync(r => r.Rating), 1);
                }

                stats.TopRatedRecipes = await _context.Recipes
                    .Include(r => r.Reviews)
                    .Where(r => r.Reviews.Any())
                    .Select(r => new RecipeRatingDto
                    {
                        RecipeId = r.Id,
                        RecipeName = r.Name,
                        AvgRating = Math.Round(r.Reviews.Average(rev => rev.Rating), 1),
                        ReviewsCount = r.Reviews.Count
                    })
                    .OrderByDescending(dto => dto.AvgRating)
                    .Take(3)
                    .ToListAsync();
            }
            catch
            {
                stats.TotalReviews = 0;
                stats.AverageRating = 0;
                stats.TopRatedRecipes = new System.Collections.Generic.List<RecipeRatingDto>();
            }

            stats.MostPopularCategory = "Premium"; // Затичка для простоти

            return View(stats);
        }
    }
}