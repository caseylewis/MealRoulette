using MealRoulette.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MealRoulette.Services
{
    public class MealService
    {
        private readonly MealDatabase _database;
        private readonly List<Meal> _default_meals = new()
        {
            new Meal { Name = "Spaghetti", MaxTimesPerWeek = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Pasta", Amount = 8, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Tomato Sauce", Amount = 1, UnitOfMeasurement = "c" },
                new Ingredient { Name = "Meatballs", Amount = 6, UnitOfMeasurement = "count" } } },
            new Meal { Name = "Tacos", MaxTimesPerWeek = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Tortillas", Amount = 3, UnitOfMeasurement = "pc" },
                new Ingredient { Name = "Beef", Amount = 4, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Cheese", Amount = 0.25, UnitOfMeasurement = "c" } } },
            new Meal { Name = "Chicken Stir Fry", MaxTimesPerWeek = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Chicken", Amount = 6, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Vegetables", Amount = 2, UnitOfMeasurement = "c" },
                new Ingredient { Name = "Soy Sauce", Amount = 2, UnitOfMeasurement = "tbsp" } } },
            new Meal { Name = "Pizza", MaxTimesPerWeek = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Dough", Amount = 1, UnitOfMeasurement = "lb" },
                new Ingredient { Name = "Cheese", Amount = 1, UnitOfMeasurement = "c" },
                new Ingredient { Name = "Pepperoni", Amount = 12, UnitOfMeasurement = "slice" } } },
            new Meal { Name = "Salmon Salad", MaxTimesPerWeek = 1, CanBeBreakfast=false, CanBeLunch=false, CanBeDinner=true,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Salmon", Amount = 4, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Lettuce", Amount = 2, UnitOfMeasurement = "c" },
                new Ingredient { Name = "Dressing", Amount = 2, UnitOfMeasurement = "tbsp" } } },
            new Meal { Name = "Egg Sandwich", MaxTimesPerWeek = 3, CanBeBreakfast=true, CanBeLunch=false, CanBeDinner=false,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Egg", Amount = 2, UnitOfMeasurement = "pc" },
                new Ingredient { Name = "Pita", Amount = 1, UnitOfMeasurement = "pc" } } },
            new Meal { Name = "Turkey Sandwich", MaxTimesPerWeek = 2, CanBeBreakfast=false, CanBeLunch=true, CanBeDinner=false,
                Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Turkey", Amount = 3, UnitOfMeasurement = "oz" },
                new Ingredient { Name = "Bread", Amount = 2, UnitOfMeasurement = "slice" } } },
        };

        public MealService(MealDatabase database)
        {
            _database = database;
        }

        // Ensure DB is initialized and migrated; seed defaults if empty
        public Task InitializeAsync() => _database.InitializeAsync(_default_meals);

        public Task<List<Meal>> GetMealsAsync() => _database.GetMealsAsync();
        public Task<Meal> GetMealByNameAsync(string name) => _database.GetMealByNameAsync(name);
        public Task<int> AddMealAsync(Meal meal, List<Ingredient> ingredients)
        {
            meal.Ingredients = ingredients;
            return _database.SaveMealAsync(meal);
        }
        public Task<int> UpdateMealAsync(Meal meal) => _database.SaveMealAsync(meal);
        public Task<int> DeleteMealAsync(Meal meal) => _database.DeleteMealAsync(meal);

        public List<Meal> GetDefaultMeals()
        {
            // Return a deep copy to avoid accidental mutation and let DB assign unique Ids
            return _default_meals.Select(m => new Meal
            {
                Name = m.Name,
                MaxTimesPerWeek = m.MaxTimesPerWeek,
                CanBeBreakfast = m.CanBeBreakfast,
                CanBeLunch = m.CanBeLunch,
                CanBeDinner = m.CanBeDinner,
                Ingredients = m.Ingredients?.Select(i => new Ingredient
                {
                    // Do NOT set Id, let SQLite auto-increment
                    Name = i.Name,
                    Amount = i.Amount,
                    UnitOfMeasurement = i.UnitOfMeasurement,
                    MealName = m.Name
                }).ToList() ?? new List<Ingredient>()
            }).ToList();
        }

        // Current week persistence API
        public Task SaveCurrentWeekAsync(WeeklyPlan plan) => _database.SaveCurrentWeekAsync(plan);
        public Task<WeeklyPlan> GetCurrentWeekAsync() => _database.GetCurrentWeekAsync();
        public Task SaveCurrentWeekGroceryAsync(Dictionary<string, GroceryAggregateItem> items) => _database.SaveCurrentWeekGroceryAsync(items);
        public Task<Dictionary<string, GroceryAggregateItem>> GetCurrentWeekGroceryAsync() => _database.GetCurrentWeekGroceryAsync();
    }
}
