using LibraryApp.Models;
using LibraryApp.Services;

namespace LibraryApp.Pages;

public partial class BooksPage : ContentPage
{
    private readonly DatabaseService _db;

    public BooksPage(DatabaseService db)
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
        BooksList.ItemsSource = await _db.GetBooksAsync();
    }

    private Book? SelectedBook => BooksList.SelectedItem as Book;

    private void Show(string msg)
    {
        Message.Text = msg;
        Message.IsVisible = true;
    }

    private async void OnAddBook(object sender, EventArgs e)
    {
        Message.IsVisible = false;
        try
        {
            var b = new Book
            {
                Title = TitleEntry.Text?.Trim() ?? "",
                Author = AuthorEntry.Text?.Trim() ?? "",
                ISBN = IsbnEntry.Text?.Trim() ?? ""
            };
            await _db.AddBookAsync(b);
            await LoadAsync();
            TitleEntry.Text = AuthorEntry.Text = IsbnEntry.Text = string.Empty;
        }
        catch (ValidationException vex) { Show(vex.Message); }
        catch (Exception ex) { Show($"Unexpected error: {ex.Message}"); }
    }

    private async void OnUpdateBook(object sender, EventArgs e)
    {
        if (SelectedBook == null) { Show("Select a book to update."); return; }
        try
        {
            SelectedBook.Title = string.IsNullOrWhiteSpace(TitleEntry.Text) ? SelectedBook.Title : TitleEntry.Text!.Trim();
            SelectedBook.Author = string.IsNullOrWhiteSpace(AuthorEntry.Text) ? SelectedBook.Author : AuthorEntry.Text!.Trim();
            SelectedBook.ISBN = string.IsNullOrWhiteSpace(IsbnEntry.Text) ? SelectedBook.ISBN : IsbnEntry.Text!.Trim();

            await _db.UpdateBookAsync(SelectedBook);
            await LoadAsync();
        }
        catch (ValidationException vex) { Show(vex.Message); }
        catch (Exception ex) { Show($"Unexpected error: {ex.Message}"); }
    }

    private async void OnDeleteBook(object sender, EventArgs e)
    {
        if (SelectedBook == null) { Show("Select a book to delete."); return; }
        try
        {
            await _db.DeleteBookAsync(SelectedBook);
            await LoadAsync();
        }
        catch (Exception ex) { Show($"Unexpected error: {ex.Message}"); }
    }
}
