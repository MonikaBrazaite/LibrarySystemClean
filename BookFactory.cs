public static class BookFactory
{
    public static Book Create(string title, string author, string isbn, int year, int categoryId)
    {
        return new Book
        {
            Title = title,
            Author = author,
            ISBN = isbn,
            YearPublished = year,
            CategoryId = categoryId
        };
    }
}
