using System.ComponentModel.DataAnnotations;

namespace RecipeBookMVC.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
}
