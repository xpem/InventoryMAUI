using ApiRepos;
using ApiRepos.Interfaces;
using CommunityToolkit.Maui;
using Inventory.Infra.Services;
using Inventory.ViewModels;
using Inventory.ViewModels.Category;
using Inventory.ViewModels.Category.SubCategory;
using Inventory.ViewModels.Item;
using Inventory.ViewModels.Item.Selectors;
using Inventory.Views;
using Inventory.Views.Category;
using Inventory.Views.Category.SubCategory;
using Inventory.Views.Item;
using Inventory.Views.Item.Selectors;
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
            MauiAppBuilder builder = MauiApp.CreateBuilder();
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
            services.AddTransientWithShellRoute<FirstSync, FirstSyncVM>(nameof(FirstSync));
            services.AddTransientWithShellRoute<Main, MainVM>(nameof(Main));
            services.AddTransientWithShellRoute<UpdatePassword, UpdatePasswordVM>(nameof(UpdatePassword));
            services.AddTransientWithShellRoute<ItemDisplay, ItemDisplayVM>(nameof(ItemDisplay));
            services.AddTransientWithShellRoute<ItemEdit, ItemEditVM>(nameof(ItemEdit));
            services.AddTransientWithShellRoute<CategorySelector, CategorySelectorVM>(nameof(CategorySelector));
            services.AddTransientWithShellRoute<SubCategorySelector, SubCategorySelectorVM>(nameof(SubCategorySelector));
            services.AddTransientWithShellRoute<CategoryList, CategoryListVM>(nameof(CategoryList));
            services.AddTransientWithShellRoute<CategoryDisplay, CategoryDisplayVM>(nameof(CategoryDisplay));
            services.AddTransientWithShellRoute<CategoryEdit, CategoryEditVM>(nameof(CategoryEdit));
            services.AddTransientWithShellRoute<SubCategoryEdit, SubCategoryEditVM>(nameof(SubCategoryEdit));

            return services;
        }

        public static IServiceCollection AddApiRepos(this IServiceCollection services)
        {
            services.AddScoped<IHttpClientFunctions, HttpClientFunctions>();
            services.AddScoped<IHttpClientWithFileFunctions, HttpClientWithFileFunctions>();

            services.AddTransient<IUserApiRepo, UserApiRepo>();
            services.AddTransient<IItemApiRepo, ItemApiRepo>();
            services.AddTransient<IItemSituationApiRepo, ItemSituationApiRepo>();

            services.AddScoped<IOperationQueueRepo, OperationQueueRepo>();
            services.AddScoped<IUserApiRepo, UserApiRepo>();
            services.AddScoped<ICategoryApiRepo, CategoryApiRepo>();
            services.AddScoped<ISubCategoryApiRepo, SubCategoryApiRepo>();
            services.AddScoped<IAcquisitionTypeApiRepo, AcquisitionTypeApiRepo>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISyncService, SyncService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBuildDbService, BuildDbService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IItemSituationService, ItemSituationService>();
            services.AddTransient<ISubCategoryService, SubCategoryService>();

            services.AddScoped<IOperationService, OperationService>();
            //services.AddScoped<ICheckServerBLL, CheckServerBLL>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAcquisitionTypeService, AcquisitionTypeService>();

            return services;
        }

        public static IServiceCollection AddLocalRepos(this IServiceCollection services)
        {
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddScoped<ISubCategoryRepo, SubCategoryRepo>();

            return services;
        }
    }
}
