using SQLite;
using MealRoulette.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MealRoulette.Services
{
    public class MealDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public MealDatabase(string dbPath, IEnumerable<Meal> defaultMeals = null)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Meal>().Wait();
            _database.CreateTableAsync<Ingredient>().Wait();

            // Seed default meals if database is empty
            if (defaultMeals != null)
            {
                var mealCount = _database.Table<Meal>().CountAsync().Result;
                if (mealCount == 0)
                {
                    foreach (var meal in defaultMeals)
                    {
                        _database.InsertAsync(meal).Wait();
                        if (meal.Ingredients != null)
                        {
                            foreach (var ingredient in meal.Ingredients)
                            {
                                ingredient.MealName = meal.Name;
                                _database.InsertAsync(ingredient).Wait();
                            }
                        }
                    }
                }
            }
        }

        public async Task<List<Meal>> GetMealsAsync()
        {
            var meals = await _database.Table<Meal>().ToListAsync();
            foreach (var meal in meals)
            {
                meal.Ingredients = await GetIngredientsForMealAsync(meal.Name);
            }
            return meals;
        }

        public async Task<Meal> GetMealByNameAsync(string name)
        {
            var meal = await _database.Table<Meal>().Where(m => m.Name == name).FirstOrDefaultAsync();
            if (meal != null)
            {
                meal.Ingredients = await GetIngredientsForMealAsync(meal.Name);
            }
            return meal;
        }

        public async Task<int> SaveMealAsync(Meal meal)
        {
            // Save meal
            var result = await _database.InsertOrReplaceAsync(meal);
            // Remove old ingredients
            var oldIngredients = await GetIngredientsForMealAsync(meal.Name);
            foreach (var ing in oldIngredients)
            {
                await _database.DeleteAsync(ing);
            }
            // Save new ingredients
            if (meal.Ingredients != null)
            {
                foreach (var ingredient in meal.Ingredients)
                {
                    ingredient.MealName = meal.Name;
                    await _database.InsertOrReplaceAsync(ingredient);
                }
            }
            return result;
        }

        public async Task<int> DeleteMealAsync(Meal meal)
        {
            // Delete ingredients first
            var ingredients = await GetIngredientsForMealAsync(meal.Name);
            foreach (var ing in ingredients)
            {
                await _database.DeleteAsync(ing);
            }
            // Delete meal
            return await _database.DeleteAsync(meal);
        }

        public Task<List<Ingredient>> GetIngredientsForMealAsync(string mealName)
        {
            return _database.Table<Ingredient>().Where(i => i.MealName == mealName).ToListAsync();
        }

        public Task<int> SaveIngredientAsync(Ingredient ingredient)
        {
            return _database.InsertOrReplaceAsync(ingredient);
        }
    }
}
