using LibraryApp.Pages;

namespace LibraryApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Ensure routes work for navigation by name
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(BooksPage), typeof(BooksPage));
        Routing.RegisterRoute(nameof(MembersPage), typeof(MembersPage));
        Routing.RegisterRoute(nameof(LoansPage), typeof(LoansPage));
    }
}
