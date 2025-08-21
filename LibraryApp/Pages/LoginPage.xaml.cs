namespace LibraryApp.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(Username.Text) || string.IsNullOrWhiteSpace(Password.Text))
        {
            ErrorLabel.Text = "Username and password are required.";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Demo-only credentials
        if (Username.Text == "admin" && Password.Text == "admin")
        {
            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        }
        else
        {
            ErrorLabel.Text = "Invalid credentials.";
            ErrorLabel.IsVisible = true;
        }
    }
}
