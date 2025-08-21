using SQLite;

namespace LibraryApp.Models;

public abstract class Person
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";

    public virtual string DisplayInfo() => $"{Name} ({Email})";
}
