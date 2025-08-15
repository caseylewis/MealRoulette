using MealRoulette.Models;

namespace MealRoulette.Services
{
    public class MealService
    {
        private readonly List<Meal> _meals = new()
        {
            new Meal { Name = "Spaghetti", DesiredMonthly = 3, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Pasta", Amount = 8, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Tomato Sauce", Amount = 1, UnitOfMeasurement = "cup" },
                new Ingredient { Name = "Meatballs", Amount = 6, UnitOfMeasurement = "count" } } },
            new Meal { Name = "Tacos", DesiredMonthly = 2, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Tortillas", Amount = 3, UnitOfMeasurement = "piece" },
                new Ingredient { Name = "Beef", Amount = 4, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Cheese", Amount = 0.25, UnitOfMeasurement = "cup" } } },
            new Meal { Name = "Chicken Stir Fry", DesiredMonthly = 2, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Chicken", Amount = 6, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Vegetables", Amount = 2, UnitOfMeasurement = "cup" },
                new Ingredient { Name = "Soy Sauce", Amount = 2, UnitOfMeasurement = "tbsp" } } },
            new Meal { Name = "Pizza", DesiredMonthly = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Dough", Amount = 1, UnitOfMeasurement = "lb" },
                new Ingredient { Name = "Cheese", Amount = 1, UnitOfMeasurement = "cup" },
                new Ingredient { Name = "Pepperoni", Amount = 12, UnitOfMeasurement = "slice" } } },
            new Meal { Name = "Salmon Salad", DesiredMonthly = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Salmon", Amount = 4, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Lettuce", Amount = 2, UnitOfMeasurement = "cup" },
                new Ingredient { Name = "Dressing", Amount = 2, UnitOfMeasurement = "tbsp" } } },
            new Meal { Name = "Egg Sandwich", DesiredMonthly = 1, CanBeBreakfast=true, CanBeLunch=false, CanBeDinner=false,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Egg", Amount = 2, UnitOfMeasurement = "piece" },
                new Ingredient { Name = "Pita", Amount = 1, UnitOfMeasurement = "piece" } } },
            new Meal { Name = "Turkey Sandwich", DesiredMonthly = 1, CanBeBreakfast=false, CanBeLunch=true, CanBeDinner=false,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Turkey", Amount = 3, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Bread", Amount = 2, UnitOfMeasurement = "slice" } } },
        };

        public IReadOnlyList<Meal> GetMeals() => _meals;

        public void AddMeal(Meal meal)
        {
            _meals.Add(meal);
        }

        public bool UpdateMealByName(string name, Meal updatedMeal)
        {
            var index = _meals.FindIndex(m => m.Name == name);
            if (index == -1) return false;
            _meals[index] = updatedMeal;
            return true;
        }

        public bool DeleteMealByName(string name)
        {
            var meal = _meals.FirstOrDefault(m => m.Name == name);
            if (meal == null) return false;
            _meals.Remove(meal);
            return true;
        }

        public Meal? GetMealByName(string name)
        {
            return _meals.FirstOrDefault(m => m.Name == name);
        }
    }
}
