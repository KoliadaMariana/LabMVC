using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeBookMVC.Models;
using System.Threading.Tasks;

namespace RecipeBookMVC.Controllers;

[Authorize] // Доступно тільки для зареєстрованих користувачів
public class PremiumController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public PremiumController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        // Передаємо в View інформацію, чи є вже у користувача Premium
        ViewBag.IsPremium = user.EmailConfirmed; // Використаємо існуюче поле як прапорець Premium, або просто передамо статус
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivatePremium()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            // Для простоти демонстрації без зміни БД активуємо Premium через системне поле EmailConfirmed
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction("Index", "Home");
    }
}