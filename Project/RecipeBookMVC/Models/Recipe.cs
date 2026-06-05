using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Переконайся, що цей using є зверху

namespace RecipeBookMVC.Models;

public class Recipe
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Ingredients { get; set; } = string.Empty;

    public string Instructions { get; set; } = string.Empty;

    public int CookingTime { get; set; }

    public string Difficulty { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    // Зв'язок з категорією
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    // Зв'язок з користувачем (авторство)
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}