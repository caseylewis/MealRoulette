using MealRoulette.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealRoulette.Services
{
    public class MealService
    {
        private readonly MealDatabase _database;
        private readonly List<Meal> _default_meals = new()
        {
            new Meal { Name = "Spaghetti", DesiredMonthly = 3, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Pasta", Amount = 8, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Tomato Sauce", Amount = 1, UnitOfMeasurement = "c" },
                new Ingredient { Name = "Meatballs", Amount = 6, UnitOfMeasurement = "count" } } },
            new Meal { Name = "Tacos", DesiredMonthly = 2, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Tortillas", Amount = 3, UnitOfMeasurement = "pc" },
                new Ingredient { Name = "Beef", Amount = 4, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Cheese", Amount = 0.25, UnitOfMeasurement = "c" } } },
            new Meal { Name = "Chicken Stir Fry", DesiredMonthly = 2, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Chicken", Amount = 6, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Vegetables", Amount = 2, UnitOfMeasurement = "c" },
                new Ingredient { Name = "Soy Sauce", Amount = 2, UnitOfMeasurement = "tbsp" } } },
            new Meal { Name = "Pizza", DesiredMonthly = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Dough", Amount = 1, UnitOfMeasurement = "lb" },
                new Ingredient { Name = "Cheese", Amount = 1, UnitOfMeasurement = "c" },
                new Ingredient { Name = "Pepperoni", Amount = 12, UnitOfMeasurement = "slice" } } },
            new Meal { Name = "Salmon Salad", DesiredMonthly = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Salmon", Amount = 4, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Lettuce", Amount = 2, UnitOfMeasurement = "c" },
                new Ingredient { Name = "Dressing", Amount = 2, UnitOfMeasurement = "tbsp" } } },
            new Meal { Name = "Egg Sandwich", DesiredMonthly = 1, CanBeBreakfast=true, CanBeLunch=false, CanBeDinner=false,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Egg", Amount = 2, UnitOfMeasurement = "pc" },
                new Ingredient { Name = "Pita", Amount = 1, UnitOfMeasurement = "pc" } } },
            new Meal { Name = "Turkey Sandwich", DesiredMonthly = 1, CanBeBreakfast=false, CanBeLunch=true, CanBeDinner=false,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Turkey", Amount = 3, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Bread", Amount = 2, UnitOfMeasurement = "slice" } } },
        };

        public MealService(MealDatabase database)
        {
            _database = database;
        }

        public MealService(string dbPath)
        {
            _database = new MealDatabase(dbPath, _default_meals);
        }

        public Task<List<Meal>> GetMealsAsync() => _database.GetMealsAsync();
        public Task<Meal> GetMealByNameAsync(string name) => _database.GetMealByNameAsync(name);
        public async Task<int> AddMealAsync(Meal meal, List<Ingredient> ingredients)
        {
            meal.Ingredients = ingredients;
            return await _database.SaveMealAsync(meal);
        }
        public async Task<int> UpdateMealAsync(Meal meal)
        {
            return await _database.SaveMealAsync(meal);
        }
        public async Task<int> DeleteMealAsync(Meal meal)
        {
            return await _database.DeleteMealAsync(meal);
        }
    }
}
