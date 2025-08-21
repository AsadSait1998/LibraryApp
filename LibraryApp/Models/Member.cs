namespace LibraryApp.Models;

public class Member : Person
{
    public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
}
