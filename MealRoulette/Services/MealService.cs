using MealRoulette.Models;

namespace MealRoulette.Services
{
    public class MealService
    {
        private readonly List<Meal> _meals = new()
        {
            new Meal { Name = "Spaghetti", DesiredMonthly = 3, Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Pasta" },
                new Ingredient { Name = "Tomato Sauce" },
                new Ingredient { Name = "Meatballs" } } },
            new Meal { Name = "Tacos", DesiredMonthly = 2, Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Tortillas" },
                new Ingredient { Name = "Beef" },
                new Ingredient { Name = "Cheese" } } },
            new Meal { Name = "Chicken Stir Fry", DesiredMonthly = 2, Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Chicken" },
                new Ingredient { Name = "Vegetables" },
                new Ingredient { Name = "Soy Sauce" } } },
            new Meal { Name = "Pizza", DesiredMonthly = 1, Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Dough" },
                new Ingredient { Name = "Cheese" },
                new Ingredient { Name = "Pepperoni" } } },
            new Meal { Name = "Salmon Salad", DesiredMonthly = 1, Ingredients = new List<Ingredient> {
                new Ingredient { Name = "Salmon" },
                new Ingredient { Name = "Lettuce" },
                new Ingredient { Name = "Dressing" } } },
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
