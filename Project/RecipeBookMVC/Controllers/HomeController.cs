using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;

namespace RecipeBookMVC.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Витягуємо назви категорій, щоб заповнити випадаючий список (Select) на формі
        ViewData["Categories"] = await _context.Categories
            .Select(c => c.Name)
            .ToListAsync();

        return View();
    }
}