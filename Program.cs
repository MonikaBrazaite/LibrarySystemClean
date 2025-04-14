using System;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new LibraryContext())
        {
            var service = new LibraryService(db);

            service.SeedDatabase();
            service.DisplayAllLoans();
            service.DisplayFilteredLoans();
            service.DisplayBooksByCategory();
            service.DisplayUniqueCategories();
        }
    }
}
