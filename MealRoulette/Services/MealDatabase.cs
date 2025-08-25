using SQLite;
using MealRoulette.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;

namespace MealRoulette.Services
{
    [Table("DbMeta")]
    public class DbMeta
    {
        [PrimaryKey]
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class MealDatabase
    {
        private readonly SQLiteAsyncConnection _database;
        private const int CurrentDbVersion = 3; // bump version for MaxTimesPerWeek migration
        private const string VersionKey = "DbVersion";
        private const string CurrentWeekKey = "CurrentWeekPlan";
        private const string CurrentWeekGroceryKey = "CurrentWeekGrocery";
        private bool _initialized = false;

        public MealDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeAsync(IEnumerable<Meal> defaultMeals = null)
        {
            if (_initialized) return;
            _initialized = true;

            // Ensure tables exist (will no-op if they already exist)
            await _database.CreateTableAsync<Meal>();
            await _database.CreateTableAsync<Ingredient>();
            await _database.CreateTableAsync<DbMeta>();

            // Migration logic
            var version = await GetDbVersionAsync();
            if (version < CurrentDbVersion)
            {
                await MigrateDatabase(version, CurrentDbVersion);
                await SetDbVersionAsync(CurrentDbVersion);
            }

            // Seed default meals if database is empty
            var mealCount = await _database.Table<Meal>().CountAsync();
            if (defaultMeals != null && mealCount == 0)
            {
                foreach (var meal in defaultMeals)
                {
                    await _database.InsertAsync(meal);
                    if (meal.Ingredients != null)
                    {
                        foreach (var ingredient in meal.Ingredients)
                        {
                            ingredient.MealName = meal.Name;
                            ingredient.Id = 0; // ensure auto-increment
                            await _database.InsertAsync(ingredient);
                        }
                    }
                }
            }
        }

        private async Task<int> GetDbVersionAsync()
        {
            var meta = await _database.Table<DbMeta>().Where(m => m.Key == VersionKey).FirstOrDefaultAsync();
            return meta != null && int.TryParse(meta.Value, out var v) ? v : 0;
        }

        private async Task SetDbVersionAsync(int version)
        {
            var meta = new DbMeta { Key = VersionKey, Value = version.ToString() };
            await _database.InsertOrReplaceAsync(meta);
        }

        private async Task MigrateDatabase(int oldVersion, int newVersion)
        {
            // Example migration to ensure Ingredients has an auto-increment PK Id and allows multiple rows per meal
            if (oldVersion < 2)
            {
                // Inspect current Ingredients table
                var cols = await _database.GetTableInfoAsync("Ingredients");
                var hasId = cols.Any(c => c.Name == nameof(Ingredient.Id));

                if (!hasId)
                {
                    // Rebuild Ingredients table without losing data
                    await _database.ExecuteAsync("CREATE TABLE IF NOT EXISTS Ingredients_New (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Amount REAL, UnitOfMeasurement TEXT, MealName TEXT)");

                    // If old Ingredients exists, copy data
                    if (cols?.Count > 0)
                    {
                        await _database.ExecuteAsync("INSERT INTO Ingredients_New (Name, Amount, UnitOfMeasurement, MealName) SELECT Name, Amount, UnitOfMeasurement, MealName FROM Ingredients");
                        await _database.ExecuteAsync("DROP TABLE IF EXISTS Ingredients");
                    }

                    await _database.ExecuteAsync("ALTER TABLE Ingredients_New RENAME TO Ingredients");
                }
            }

            if (oldVersion < 3)
            {
                // Handle Meals column rename from DesiredMonthly to MaxTimesPerWeek if needed
                var mealCols = await _database.GetTableInfoAsync("Meals");
                var hasDesiredMonthly = mealCols.Any(c => c.Name == "DesiredMonthly");
                var hasMaxPerWeek = mealCols.Any(c => c.Name == "MaxTimesPerWeek");
                if (hasDesiredMonthly && !hasMaxPerWeek)
                {
                    // Add new column and copy data
                    await _database.ExecuteAsync("ALTER TABLE Meals ADD COLUMN MaxTimesPerWeek INTEGER");
                    await _database.ExecuteAsync("UPDATE Meals SET MaxTimesPerWeek = COALESCE(DesiredMonthly, 1)");
                    // Optionally leave old column; SQLite doesn't support drop column easily without table rebuild
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
            
            // Save new ingredients (always insert fresh rows)
            if (meal.Ingredients != null)
            {
                foreach (var ingredient in meal.Ingredients)
                {
                    ingredient.MealName = meal.Name;
                    ingredient.Id = 0; // reset so SQLite assigns a new Id
                    await _database.InsertAsync(ingredient);
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
            // For direct saves elsewhere, prefer Insert for new items
            if (ingredient.Id == 0)
            {
                ingredient.Id = 0;
                return _database.InsertAsync(ingredient);
            }
            return _database.InsertOrReplaceAsync(ingredient);
        }

        // --- Current week persistence ---
        public async Task SaveCurrentWeekAsync(WeeklyPlan plan)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(plan);
            await _database.InsertOrReplaceAsync(new DbMeta { Key = CurrentWeekKey, Value = json });
        }

        public async Task<WeeklyPlan> GetCurrentWeekAsync()
        {
            var meta = await _database.Table<DbMeta>().Where(m => m.Key == CurrentWeekKey).FirstOrDefaultAsync();
            if (meta == null || string.IsNullOrWhiteSpace(meta.Value)) return null;
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<WeeklyPlan>(meta.Value);
            }
            catch
            {
                return null;
            }
        }

        public async Task SaveCurrentWeekGroceryAsync(Dictionary<string, GroceryAggregateItem> items)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(items);
            await _database.InsertOrReplaceAsync(new DbMeta { Key = CurrentWeekGroceryKey, Value = json });
        }

        public async Task<Dictionary<string, GroceryAggregateItem>> GetCurrentWeekGroceryAsync()
        {
            var meta = await _database.Table<DbMeta>().Where(m => m.Key == CurrentWeekGroceryKey).FirstOrDefaultAsync();
            if (meta == null || string.IsNullOrWhiteSpace(meta.Value)) return new();
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, GroceryAggregateItem>>(meta.Value) ?? new();
            }
            catch
            {
                return new();
            }
        }
    }

    // Lightweight DTO for grocery storage
    public class GroceryAggregateItem
    {
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double TotalAmount { get; set; }
        public bool Checked { get; set; } = true;
    }
}
