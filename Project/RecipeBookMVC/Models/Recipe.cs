using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBookMVC.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa przepisu jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa nie może być dłuższa niż 100 znaków.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Składniki są wymagane.")]
        public string Ingredients { get; set; }

        [Required(ErrorMessage = "Instrukcje są wymagane.")]
        public string Instructions { get; set; }

        [Required(ErrorMessage = "Czas przygotowania jest wymagany.")]
        [Range(1, 600, ErrorMessage = "Czas musi być w przedziale od 1 do 600 minut.")]
        public int CookingTime { get; set; }

        [Required(ErrorMessage = "Poziom trudności jest wymagany.")]
        public string Difficulty { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}