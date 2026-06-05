using System.Collections.Generic;

namespace RecipeBookMVC.Models
{
    public class DashboardViewModel
    {
        public int TotalRecipes { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public string MostPopularCategory { get; set; } = string.Empty;
        public List<RecipeRatingDto> TopRatedRecipes { get; set; } = new List<RecipeRatingDto>();
    }

    public class RecipeRatingDto
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public double AvgRating { get; set; }
        public int ReviewsCount { get; set; }
    }
}