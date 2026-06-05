using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeBookMVC.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole 'Treść' jest wymagane.")]
        [Display(Name = "Komentarz")]
        public string Content { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Ocena musi być w przedziale od 1 do 5.")]
        [Display(Name = "Ocena")]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Зв'язок із рецептом: до якого саме рецепта належить цей відгук
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}