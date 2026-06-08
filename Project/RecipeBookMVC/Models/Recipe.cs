using System.Collections.Generic;

namespace RecipeBookMVC.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public int CookingTime { get; set; }

        // Обов'язково має бути цей рядок:
        public string Difficulty { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Обов'язково має бути цей рядок:
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}