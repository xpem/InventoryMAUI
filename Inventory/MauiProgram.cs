using CommunityToolkit.Maui;
using Inventory.ViewModels;
using Inventory.Views;
using Microsoft.Extensions.Logging;

namespace Inventory
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FA6Free-Solid-900.otf", "Icons");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddViewsViewModels();

            return builder.Build();
        }

        public static IServiceCollection AddViewsViewModels(this IServiceCollection services)
        {
            services.AddTransientWithShellRoute<SignIn, SignInVM>(nameof(SignIn));

            return services;
        }
    }
}
