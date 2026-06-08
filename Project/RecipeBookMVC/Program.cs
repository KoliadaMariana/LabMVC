using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeBookMVC.Data;
using RecipeBookMVC.Models;
using System;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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
        Name = "Kurczak Curry z Ryżem",
        Ingredients = "Kurczak, ryż basmati, mleczko kokosowe, pasta curry, cebula, czosnek, olej, sól, pieprz",
        Instructions = "Ugotuj ryż basmati według instrukcji na opakowaniu. Na patelni rozgrzej olej, podsmaż posiekaną cebulę i czosnek. Dodaj pokrojonego w kostkę kurczaka, dopraw solą oraz pieprzem i smaż do złocistego koloru. Następnie dodaj łyżkę pasty curry, wymieszaj i wlej mleczko kokosowe. Całość gulaszu duś na małym ogniu przez około 15 minut, aż sos ładnie zgęstnieje. Podawaj gorące z ryżem.",
        CookingTime = 30,
        Difficulty = "Łatwe",
        CategoryId = obiady.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1588166524941-3bf61a9c41db?w=800"
    };

    var recipe2 = new Recipe
    {
        Name = "Puszyste Brownie Czekoladowe",
        Ingredients = "Czekolada gorzka, masło, jajka, mąka pszenna, cukier, proszek do pieczenia",
        Instructions = "W małym garnuszku rozpuść masło razem z połamaną gorzką czekoladą na małym ogniu, stale mieszając, a następnie odstaw do wystygnięcia. W osobnej misce ubij mikserem jajka z cukrem na puszystą masę. Powoli wlej przestudzoną czekoladę do ubitych jajek. Na koniec wsyp przesianą mąkę z odrobiną proszku do pieczenia i delikatnie wymieszaj łyżką. Przelej ciasto do formy i piecz w piekarniku nagrzanym do 180 stopni przez 25 minut.",
        CookingTime = 45,
        Difficulty = "Średnie",
        CategoryId = desery.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1606313564200-e75d5e30476c?w=800"
    };

    var recipe3 = new Recipe
    {
        Name = "Domowa Zupa Pomidorowa",
        Ingredients = "Pomidory w puszce, bulion warzywny, makaron, śmietana 18%, marchewka, pietruszka, sól, pieprz",
        Instructions = "Do garnka wlej bulion warzywny, dodaj pokrojoną w kostkę marchewkę oraz pietruszkę i gotuj do miękkości. Następnie dodaj zblendowane pomidory z puszki i gotuj całość przez kolejne 10 minut. W osobnym garnku ugotuj ulubiony makaron. Pod koniec gotowania zupy, zahartuj śmietanę odrobiną gorącego wywaru, wlej do garnka i dokładnie wymieszaj. Dopraw solą oraz pieprzem. Podawaj z makaronem.",
        CookingTime = 40,
        Difficulty = "Bardzo łatwe",
        CategoryId = zupy.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=800"
    };

    var recipe4 = new Recipe
    {
        Name = "Puszyste Naleśniki z Twarogiem",
        Ingredients = "Mąka pszenna, mleko, jajka, woda gazowana, twaróg, cukier waniliowy, masło",
        Instructions = "W misce wymieszaj mąkę, mleko, jajka oraz wodę gazowaną na gładkie ciasto bez grudek. Smaż cienkie naleśniki na dobrze rozgrzanej patelni z odrobiną masła z obu stron na złoty kolor. Twaróg rozgnieć widelcem i wymieszaj z cukrem waniliowym. Nakładaj przygotowany farsz serowy na każdy naleśnik, zwiń w rulon lub trójkąty i podawaj.",
        CookingTime = 25,
        Difficulty = "Łatwe",
        CategoryId = sniadania.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=800"
    };

    var recipe5 = new Recipe
    {
        Name = "Orzeźwiająca Lemoniada Cytrynowa",
        Ingredients = "Cytryna, woda, miód, mięta, lód",
        Instructions = "Wyciśnij świeży sok z kilku cytryn i wlej go do dużego dzbanka. Dodaj kilka łyżek płynnego miodu i dokładnie wymieszaj, aż miód całkowicie się rozpuści. Następnie wlej zimną wodę mineralną. Wrzuć umyte listki świeżej miętą oraz plasterki cytryny dla ozdoby. Przed podaniem dodaj kostki lodu, aby napój był mocno schłodzony.",
        CookingTime = 10,
        Difficulty = "Bardzo łatwe",
        CategoryId = napoje.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1513558161293-cdaf765ed2fd?w=800"
    };

    var recipe6 = new Recipe
    {
        Name = "Klasyczna Jajecznica ze Szczypiorkiem",
        Ingredients = "Jajka, masło, szczypiorek, sól, pieprz",
        Instructions = "Na patelni rozpuść łyżeczkę masła na małym ogniu. Wbij jajka bezpośrednio na patelnię lub roztrzep je wcześniej w miseczce. Smaż powoli, delikatnie mieszając drewnianą łopatką, aż jajka osiągną idealną для Ciebie konsystencję. Na sam koniec wyłącz ogień, dopraw jajecznicę solą, pieprzem i posyp obficie świeżo posiekaným szczypiorkiem.",
        CookingTime = 10,
        Difficulty = "Bardzo łatwe",
        CategoryId = sniadania.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1525351484163-7529414344d8?w=800"
    };

    var recipe7 = new Recipe
    {
        Name = "Włoski Makaron Carbonara",
        Ingredients = "Makaron spaghetti, boczek, jajka, parmezan, czosnek, sól, pieprz",
        Instructions = "Ugotuj makaron spaghetti al dente w osolonej wodzie. W międzyczasie na suchej patelni podsmaż pokrojony w kostkę boczek z całym ząbkiem czosnku, który potem wyjmiesz. W miseczce wymieszaj żółtka jajek z drobno startym parmezanem i dużą ilością pieprzu. Wrzuć gorący makaron bezpośrednio na patelnię z boczkiem, zdejmij z ognia, wlej masę jajeczną i szybko wymieszaj, tworząc kremowy sos.",
        CookingTime = 20,
        Difficulty = "Średnie",
        CategoryId = obiady.Id,
        UserId = adminUser.Id,
        ImageUrl = "https://images.unsplash.com/photo-1612874742237-6526221588e3?w=800"
    };

    context.Recipes.AddRange(recipe1, recipe2, recipe3, recipe4, recipe5, recipe6, recipe7);
    await context.SaveChangesAsync();

    var reviews = new List<Review>
    {
        new Review { Content = "Pyszne danie! Polecam wszystkim.", Rating = 5, CreatedAt = DateTime.Now, RecipeId = recipe1.Id },
        new Review { Content = "Trochę za słodkie, ale ogólnie super.", Rating = 4, CreatedAt = DateTime.Now, RecipeId = recipe2.Id },
        new Review { Content = "Klasyczny smak, idealna na obiad.", Rating = 5, CreatedAt = DateTime.Now, RecipeId = recipe3.Id },
        new Review { Content = "Super śniadanie, dzieciaki zjadły ze smakiem!", Rating = 5, CreatedAt = DateTime.Now, RecipeId = recipe4.Id },
        new Review { Content = "Bardzo dobrze chłodzi w upalne dni.", Rating = 5, CreatedAt = DateTime.Now, RecipeId = recipe5.Id }
    };

    context.Reviews.AddRange(reviews);
    await context.SaveChangesAsync();
}

app.Run();