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
