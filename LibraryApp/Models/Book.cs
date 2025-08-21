using SQLite;

namespace LibraryApp.Models;

public class Book : IBorrowable
{
    [PrimaryKey, AutoIncrement]
    public int BookId { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string ISBN { get; set; } = "";
    public string Status { get; set; } = "Available"; // Available | Borrowed

    public Task BorrowItemAsync()
    {
        if (Status == "Borrowed")
            throw new InvalidOperationException("Book is already borrowed.");
        Status = "Borrowed";
        return Task.CompletedTask;
    }

    public Task ReturnItemAsync()
    {
        if (Status == "Available")
            throw new InvalidOperationException("Book is not currently borrowed.");
        Status = "Available";
        return Task.CompletedTask;
    }
}
