using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _db;

    public BookRepository(LibraryContext db)
    {
        _db = db;
    }

    public void AddBooks(List<Book> books)
    {
        _db.Books.AddRange(books);
        _db.SaveChanges();
    }

    public List<Book> GetAllBooks()
    {
        return _db.Books.Include(b => b.Category).ToList();
    }
}
