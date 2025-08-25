using SQLite;

namespace MealRoulette.Models;

[Table("Meals")]
public class Meal
{
    [PrimaryKey]
    public string Name { get; set; }
    // Ingredients will be handled separately in the DB
    [Ignore]
    public List<Ingredient> Ingredients { get; set; } = new();
    public int MaxTimesPerWeek { get; set; } = 1;
    public bool CanBeBreakfast { get; set; } = false;
    public bool CanBeLunch { get; set; } = false;
    public bool CanBeDinner { get; set; } = false;
}