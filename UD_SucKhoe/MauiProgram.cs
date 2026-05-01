using Microsoft.Extensions.Logging;
using UD_SucKhoe.Services.Database;
using UD_SucKhoe.Services.Nutrition;

namespace UD_SucKhoe
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<NutritionPage>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<INutritionService, NutritionService>();
            builder.Services.AddSingleton<INutritionService, NutritionService>();
            var app = builder.Build();
            App.Services = app.Services;
            return builder.Build();
        }
    }
}
