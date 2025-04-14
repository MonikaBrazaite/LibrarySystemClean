using System.Collections.Generic;

public interface IBookRepository
{
    void AddBooks(List<Book> books);
    List<Book> GetAllBooks();
}
