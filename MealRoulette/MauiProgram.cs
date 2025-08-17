using MealRoulette.Services;
using Microsoft.Extensions.Logging;
using System.IO;

namespace MealRoulette
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Register MealDatabase as singleton
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "meals.db3");
            builder.Services.AddSingleton(x => new MealDatabase(dbPath));
            builder.Services.AddSingleton<MealService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
