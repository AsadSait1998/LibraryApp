using LibraryApp.Models;
using LibraryApp.Services;

namespace LibraryApp.Pages;

public partial class LoansPage : ContentPage
{
    private readonly DatabaseService _db;

    public LoansPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        MemberPicker.ItemsSource = await _db.GetMembersAsync();
        BookPicker.ItemsSource = await _db.GetBooksAsync();
        ActiveLoans.ItemsSource = await _db.GetActiveLoansAsync();
    }

    private void Show(string msg)
    {
        Message.Text = msg;
        Message.IsVisible = true;
    }

    private async void OnBorrow(object sender, EventArgs e)
    {
        Message.IsVisible = false;
        var member = MemberPicker.SelectedItem as Member;
        var book = BookPicker.SelectedItem as Book;

        if (member == null) { Show("Select a member."); return; }
        if (book == null) { Show("Select a book."); return; }

        try
        {
            await _db.BorrowAsync(book.BookId, member.Id);
            await LoadAsync();
        }
        catch (ValidationException vex) { Show(vex.Message); }
        catch (Exception ex) { Show($"Unexpected error: {ex.Message}"); }
    }

    private async void OnReturn(object sender, EventArgs e)
    {
        Message.IsVisible = false;
        var loan = ActiveLoans.SelectedItem as Loan;
        if (loan == null) { Show("Select an active loan to return."); return; }

        try
        {
            await _db.ReturnAsync(loan.LoanId);
            await LoadAsync();
        }
        catch (ValidationException vex) { Show(vex.Message); }
        catch (Exception ex) { Show($"Unexpected error: {ex.Message}"); }
    }
}
