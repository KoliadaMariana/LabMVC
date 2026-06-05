using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;
using System;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// 1. ПІDТРИМКА MVC
builder.Services.AddControllersWithViews();

// 2. БД SQLITE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. НАЛАШТУВАННЯ IDENTITY
builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 4. НАЛАШТУВАННЯ КУКІВ
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 5. SEED DATA - ВИПРАВЛЕНО ТИП DATETIME
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    var adminUser = new ApplicationUser
    {
        UserName = "mariana@smakolyk.pl",
        Email = "mariana@smakolyk.pl",
        FirstName = "Mariana",
        LastName = "Koliada",
        EmailConfirmed = true
    };
    await userManager.CreateAsync(adminUser, "Password123!");

    var sniadania = new Category { Name = "Śniadania" };
    var zupy = new Category { Name = "Zupy" };
    var obiady = new Category { Name = "Obiady" };
    var desery = new Category { Name = "Desery" };
    var napoje = new Category { Name = "Napoje" };

    context.Categories.AddRange(sniadania, zupy, obiady, desery, napoje);
    await context.SaveChangesAsync();

    var recipe1 = new Recipe
    {
        Name = "Kurczak Curry",
        Ingredients = "Kurczak, ryż, mleczko kokosowe, curry",
        Instructions = "Podsmaż kurczaka, dodaj curry i mleczko. Gotuj 15 min.",
        CookingTime = 30,
        Difficulty = "Łatwe",
        CategoryId = obiady.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1588166524941-3bf61a9c41db?w=800"
    };

    var recipe2 = new Recipe
    {
        Name = "Puszyste Brownie",
        Ingredients = "Czekolada, masło, jajka, mąka",
        Instructions = "Rozpuść czekoladę, dodaj resztę. Piecz 25 min.",
        CookingTime = 45,
        Difficulty = "Średnie",
        CategoryId = desery.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1606313564200-e75d5e30476c?w=800"
    };

    var recipe3 = new Recipe
    {
        Name = "Zupa Pomidorowa",
        Ingredients = "Pomidory, bulion, makaron, śmietana",
        Instructions = "Gotuj bulion z pomidorami, zabiel śmietaną.",
        CookingTime = 40,
        Difficulty = "Bardzo łatwe",
        CategoryId = zupy.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=800"
    };

    context.Recipes.AddRange(recipe1, recipe2, recipe3);
    await context.SaveChangesAsync();

    // Тут тепер передається чистий DateTime.Now замість тексту
    var reviews = new List<Review>
    {
        new Review { Content = "Pyszne danie! Polecam wszystkim.", Rating = 5, CreatedAt = DateTime.Now, RecipeId = recipe1.Id },
        new Review { Content = "Trochę za słodkie, ale ogólnie super.", Rating = 4, CreatedAt = DateTime.Now, RecipeId = recipe2.Id },
        new Review { Content = "Klasyczny smak, idealna na obiad.", Rating = 5, CreatedAt = DateTime.Now, RecipeId = recipe3.Id }
    };

    context.Reviews.AddRange(reviews);
    await context.SaveChangesAsync();
}

app.Run();