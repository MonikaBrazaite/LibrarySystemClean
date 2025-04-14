using System;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new LibraryContext())
        {
            var bookRepo = new BookRepository(db);
            var service = new LibraryService(db, bookRepo);

            service.SeedDatabase();
            service.DisplayAllLoans();
            service.DisplayFilteredLoans();
            service.DisplayBooksByCategory();
            service.DisplayUniqueCategories();
        }
    }
}
