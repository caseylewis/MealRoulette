namespace MealRoulette.Models;

public class Meal
{
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new();
    public int DesiredMonthly { get; set; }
    public bool CanBeBreakfast { get; set; } = false;
    public bool CanBeLunch { get; set; } = false;
    public bool CanBeDinner { get; set; } = false;
}