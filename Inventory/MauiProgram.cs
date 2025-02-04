using ApiRepos;
using ApiRepos.Interfaces;
using CommunityToolkit.Maui;
using Inventory.ViewModels;
using Inventory.Views;
using LocalRepos;
using LocalRepos.Interface;
using Microsoft.Extensions.Logging;
using Services;
using Services.Interfaces;

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

            builder.Services.AddDbContextFactory<InventoryDbContext>();

            builder.Services.AddViewsViewModels();

            builder.Services.AddApiRepos();

            builder.Services.AddLocalRepos();

            builder.Services.AddServices();

            return builder.Build();
        }

        public static IServiceCollection AddViewsViewModels(this IServiceCollection services)
        {
            services.AddTransientWithShellRoute<SignIn, SignInVM>(nameof(SignIn));
            services.AddTransientWithShellRoute<SignUp, SignUpVM>(nameof(SignUp));
            services.AddTransientWithShellRoute<UpdatePassword, UpdatePasswordVM>(nameof(UpdatePassword));

            return services;
        }

        public static IServiceCollection AddApiRepos(this IServiceCollection services)
        {
            services.AddTransient<IUserApiRepo, UserApiRepo>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBuildDbService, BuildDbService>();
            return services;
        }

        public static IServiceCollection AddLocalRepos(this IServiceCollection services)
        {
            services.AddTransient<IUserRepo, UserRepo>();
            return services;
        }
    }
}
