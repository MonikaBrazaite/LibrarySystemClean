using System.Collections.Generic;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public List<Book> Books { get; set; } = new();
}
