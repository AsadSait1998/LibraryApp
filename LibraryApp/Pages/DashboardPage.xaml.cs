namespace LibraryApp.Pages;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private async void GotoBooks(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(BooksPage));

    private async void GotoMembers(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(MembersPage));

    private async void GotoLoans(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(LoansPage));
}
