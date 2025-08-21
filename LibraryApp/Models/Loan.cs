using SQLite;

namespace LibraryApp.Models;

public class Loan
{
    [PrimaryKey, AutoIncrement]
    public int LoanId { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnDate { get; set; }
}
