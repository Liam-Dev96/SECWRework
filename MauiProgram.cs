using Microsoft.Extensions.Logging;
using SECWRework.Services;
using SECWRework.Views;
using SECWRework.ViewModels;
using System.Collections.ObjectModel;

namespace SECWRework
{
    /// <summary>
    /// Configures and builds the Maui application.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Creates and configures the Maui application.
        /// </summary>
        /// <returns>A configured MauiApp instance.</returns>
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

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "SoftwareEngineering.db");
            builder.Services.AddSingleton(new BackupService(dbPath));
            builder.Services.AddSingleton<LocalDBService>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<SensorPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            var appInstance = app.Services.GetService<App>();
            if (appInstance != null)
            {
                appInstance.MainPage = new NavigationPage(new MainPage());
            }
            return app;
        }
    }
}
