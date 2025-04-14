public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int YearPublished { get; set; }

    // Foreign key
    public int CategoryId { get; set; }

    // Navigation property
    public Category Category { get; set; } = null!;
}
