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
        var categoriesList = await _context.Categories.ToListAsync();
        var categoryNames = new System.Collections.Generic.List<string>();

        foreach (var c in categoriesList)
        {
            categoryNames.Add(c.Name);
        }

        ViewData["Categories"] = categoryNames;

        return View();
    }
}