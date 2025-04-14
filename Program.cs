using System;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new LibraryContext())
        {
            var bookRepo = new BookRepository(db);
            var service = new LibraryService(db, bookRepo);
            var facade = new LibraryFacade(service);

            // Initialize system: seed data + register observers
            facade.InitializeSystem();

            // Display all info
            facade.ShowAllLoans();
            facade.ShowFilteredLoans();
            facade.ShowBooksByCategory();
            facade.ShowUniqueCategories();

            // ✅ Complex DB query (multiple filters, relationships, and parameters)
            facade.ShowComplexFilteredLoans();

            // ✅ Test CRUD functionality
            Console.WriteLine("\n--- Testing UpdateBookTitle ---");
            facade.ChangeBookTitle(1, "Updated Book Title 1");

            Console.WriteLine("\n--- Testing DeleteLoan ---");
            facade.RemoveLoan(1);

            // Display updated results
            Console.WriteLine("\n--- Loans After CRUD Changes ---");
            facade.ShowAllLoans();
        }
    }
}
