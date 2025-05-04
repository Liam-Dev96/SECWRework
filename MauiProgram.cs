using Microsoft.Extensions.Logging;
using SECWRework.Services;
using SECWRework.Views;
using SECWRework.ViewModels;
using System.Collections.ObjectModel;

namespace SECWRework
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

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "SoftwareEngineering.db");
            builder.Services.AddSingleton(new BackupService(dbPath));
            builder.Services.AddSingleton<LocalDBService>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
