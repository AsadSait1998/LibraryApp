namespace LibraryApp.Models;

public interface IBorrowable
{
    Task BorrowItemAsync();
    Task ReturnItemAsync();
}
