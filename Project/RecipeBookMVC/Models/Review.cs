using System;

namespace RecipeBookMVC.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}