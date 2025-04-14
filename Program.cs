using System;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new LibraryContext())
        {
            var bookRepo = new BookRepository(db);
            var service = new LibraryService(db, bookRepo);
            var facade = new LibraryFacade(service); // Use the Facade here

            facade.InitializeSystem();              // Seeds the database and attaches observers
            facade.ShowAllLoans();
            facade.ShowFilteredLoans();
            facade.ShowBooksByCategory();
            facade.ShowUniqueCategories();
        }
    }
}
