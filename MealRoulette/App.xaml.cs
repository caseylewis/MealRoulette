namespace MealRoulette
{
    public partial class App : Application
    {
        public App(Services.MealService mealService)
        {
            InitializeComponent();

            // Initialize DB and run migrations on app startup
            _ = mealService.InitializeAsync();

            MainPage = new MainPage();
        }
    }
}
/*
  MealRoulette Household Account Scaffold

  Purpose:
  Enable a single household to register with one shared email address, while allowing multiple individual user profiles under that account.
  Each user (e.g., Casey, Natalie) can log in using the shared household email and select meals for the week.
  The final grocery list should reflect the combined selections of all users in the household.

  Core Requirements:
  - HouseholdAccount
    - Unique email address
    - Secure password
    - List of associated UserProfiles

  - UserProfile
    - Display name (e.g., "Casey", "Natalie")
    - Preferences (e.g., dietary restrictions, favorite cuisines)
    - WeeklyMealSelections (linked to Meal entities)

  - Meal
    - Name, ingredients, tags (e.g., vegetarian, spicy)
    - Rating or weight (for spin wheel logic)
    - AssignedTo: list of UserProfiles who selected this meal

  - GroceryList
    - Auto-generated based on all selected meals for the week
    - Deduplicated and aggregated across users
    - Optionally marked with who requested each item

  Authentication Flow:
  - Login via shared HouseholdAccount email
  - Upon login, select or create a UserProfile
  - Navigate to meal selection and assign meals to individual profiles

  UI Considerations:
  - Household dashboard with switchable user views
  - Meal selection screen with profile tagging
  - Grocery list view showing merged ingredients and optional attribution

  Future Extensions:
  - Invite other household members via email
  - Role-based permissions (e.g., admin vs. contributor)
  - Shared calendar integration for meal scheduling

*/
