using SQLite;

namespace MealRoulette.Models;

[Table("Ingredients")]
public class Ingredient
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }
    public string UnitOfMeasurement { get; set; }
    public string MealName { get; set; } // Foreign key reference to Meal
}