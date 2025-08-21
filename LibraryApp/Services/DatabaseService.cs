using SQLite;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _db;
    private readonly Task _initTask; // ensure init completes before any query

    public DatabaseService()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "library.db3");
        _db = new SQLiteAsyncConnection(path);
        _initTask = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        // Create tables if they don't exist (idempotent)
        await _db.CreateTableAsync<Book>();
        await _db.CreateTableAsync<Member>();
        await _db.CreateTableAsync<Loan>();
        // If you later manage staff:
        // await _db.CreateTableAsync<Librarian>();

        // Seed books once
        if (await _db.Table<Book>().CountAsync() == 0)
        {
            await _db.InsertAllAsync(new[]
            {
                new Book { Title = "Clean Code", Author = "Robert C. Martin", ISBN = "9780132350884" },
                new Book { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", ISBN = "9780201616224" }
            });
        }
    }

    // ===== Utility =====
    private Task EnsureReadyAsync() => _initTask;

    // ===== Books =====
    public async Task<List<Book>> GetBooksAsync()
    {
        await EnsureReadyAsync();
        return await _db.Table<Book>().ToListAsync();
    }

    public async Task<Book> GetBookByIdAsync(int id)
    {
        await EnsureReadyAsync();
        return await _db.Table<Book>().Where(x => x.BookId == id).FirstOrDefaultAsync();
    }

    public async Task<int> AddBookAsync(Book b)
    {
        await EnsureReadyAsync();
        ValidateBook(b);
        return await _db.InsertAsync(b);
    }

    public async Task<int> UpdateBookAsync(Book b)
    {
        await EnsureReadyAsync();
        ValidateBook(b);
        return await _db.UpdateAsync(b);
    }

    public async Task<int> DeleteBookAsync(Book b)
    {
        await EnsureReadyAsync();
        return await _db.DeleteAsync(b);
    }

    private static void ValidateBook(Book b)
    {
        if (string.IsNullOrWhiteSpace(b.Title)) throw new ValidationException("Title is required.");
        if (string.IsNullOrWhiteSpace(b.Author)) throw new ValidationException("Author is required.");
        if (string.IsNullOrWhiteSpace(b.ISBN)) throw new ValidationException("ISBN is required.");
    }

    // ===== Members =====
    public async Task<List<Member>> GetMembersAsync()
    {
        await EnsureReadyAsync();
        return await _db.Table<Member>().ToListAsync();
    }

    public async Task<Member> GetMemberByIdAsync(int id)
    {
        await EnsureReadyAsync();
        return await _db.Table<Member>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> AddMemberAsync(Member m)
    {
        await EnsureReadyAsync();
        ValidateMember(m);
        return await _db.InsertAsync(m);
    }

    public async Task<int> UpdateMemberAsync(Member m)
    {
        await EnsureReadyAsync();
        ValidateMember(m);
        return await _db.UpdateAsync(m);
    }

    public async Task<int> DeleteMemberAsync(Member m)
    {
        await EnsureReadyAsync();
        return await _db.DeleteAsync(m);
    }

    private static void ValidateMember(Member m)
    {
        if (string.IsNullOrWhiteSpace(m.Name)) throw new ValidationException("Member name is required.");
        if (string.IsNullOrWhiteSpace(m.Email) || !m.Email.Contains("@")) throw new ValidationException("Valid email is required.");
    }

    // ===== Loans =====
    public async Task<int> BorrowAsync(int bookId, int memberId)
    {
        await EnsureReadyAsync();

        var book = await GetBookByIdAsync(bookId);
        if (book is null) throw new ValidationException("Book not found.");

        var member = await GetMemberByIdAsync(memberId);
        if (member is null) throw new ValidationException("Member not found.");

        await book.BorrowItemAsync();
        await _db.UpdateAsync(book);

        var loan = new Loan { BookId = bookId, MemberId = memberId, BorrowDate = DateTime.UtcNow };
        return await _db.InsertAsync(loan);
    }

    public async Task<int> ReturnAsync(int loanId)
    {
        await EnsureReadyAsync();

        var loan = await _db.Table<Loan>().Where(x => x.LoanId == loanId).FirstOrDefaultAsync()
                   ?? throw new ValidationException("Loan not found.");

        if (loan.ReturnDate != null) throw new ValidationException("Loan already returned.");

        var book = await GetBookByIdAsync(loan.BookId)
                   ?? throw new ValidationException("Book not found.");

        await book.ReturnItemAsync();
        await _db.UpdateAsync(book);

        loan.ReturnDate = DateTime.UtcNow;
        return await _db.UpdateAsync(loan);
    }

    public async Task<List<Loan>> GetActiveLoansAsync()
    {
        await EnsureReadyAsync();
        return await _db.Table<Loan>().Where(l => l.ReturnDate == null).ToListAsync();
    }
}
