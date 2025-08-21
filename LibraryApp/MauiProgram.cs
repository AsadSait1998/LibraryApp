using LibraryApp.Pages;
using LibraryApp.Services;

namespace LibraryApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Services
        builder.Services.AddSingleton<DatabaseService>();

        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<BooksPage>();
        builder.Services.AddTransient<MembersPage>();
        builder.Services.AddTransient<LoansPage>();

        return builder.Build();
    }
}
