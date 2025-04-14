using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class LibraryService
{
    private readonly LibraryContext _db;
    private readonly IBookRepository _bookRepo;

    private List<ILoanObserver> _observers = new List<ILoanObserver>();

    public LibraryService(LibraryContext db, IBookRepository bookRepo)
    {
        _db = db;
        _bookRepo = bookRepo;
    }

    public void RegisterObserver(ILoanObserver observer)
    {
        _observers.Add(observer);
    }

    private void NotifyObservers(Loan loan)
    {
        foreach (var observer in _observers)
        {
            observer.OnLoanCreated(loan);
        }
    }

    public void SeedDatabase()
    {
        if (!_db.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Fiction" },
                new Category { Name = "Science Fiction" },
                new Category { Name = "Fantasy" },
                new Category { Name = "Non-Fiction" },
                new Category { Name = "Mystery" },
                new Category { Name = "Biography" },
                new Category { Name = "Romance" },
                new Category { Name = "Thriller" },
                new Category { Name = "Children" },
                new Category { Name = "History" },
                new Category { Name = "Horror" },
                new Category { Name = "Poetry" },
                new Category { Name = "Science" },
                new Category { Name = "Self-help" },
                new Category { Name = "Adventure" },
                new Category { Name = "Drama" },
                new Category { Name = "Classic" },
                new Category { Name = "Travel" },
                new Category { Name = "Philosophy" },
                new Category { Name = "Health" }
            };
            _db.Categories.AddRange(categories);
            _db.SaveChanges();
            Logger.Instance.Log("Seeded 20 categories.");
        }

        if (!_db.Members.Any())
        {
            var members = Enumerable.Range(1, 20).Select(i => new Member
            {
                Name = $"Member {i}",
                Contact = new ContactInfo
                {
                    Email = $"member{i}@example.com",
                    PhoneNumber = $"123-456-78{i:D2}"
                }
            }).ToList();

            _db.Members.AddRange(members);
            _db.SaveChanges();
            Logger.Instance.Log("Seeded 20 members.");
        }

        if (!_db.Books.Any())
        {
            var categories = _db.Categories.Take(20).ToList();
            var books = Enumerable.Range(1, 20).Select(i =>
                BookFactory.Create(
                    $"Book Title {i}",
                    $"Author {i}",
                    $"978-000000000{i:D2}",
                    2000 + i,
                    categories[i % categories.Count].CategoryId
                )
            ).ToList();

            _bookRepo.AddBooks(books);
            Logger.Instance.Log("Seeded 20 books.");
        }

        if (!_db.Loans.Any())
        {
            var books = _bookRepo.GetAllBooks().Take(20).ToList();
            var members = _db.Members.Take(20).ToList();

            var loans = Enumerable.Range(0, 20).Select(i =>
            {
                var loan = new Loan
                {
                    BookId = books[i].BookId,
                    MemberId = members[i].MemberId,
                    LoanDate = DateTime.Now.AddDays(-i)
                };
                NotifyObservers(loan); // Observer pattern
                return loan;
            }).ToList();

            _db.Loans.AddRange(loans);
            _db.SaveChanges();
        }
    }

    public void DisplayAllLoans()
    {
        ILoanDisplayer displayer = new BasicLoanDisplayer(_db);
        displayer = new TimestampedLoanDisplayer(displayer); // Decorator
        displayer.Display();
    }

    public void DisplayFilteredLoans()
    {
        var filteredLoans = _db.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.Book.Title.ToLower().Contains("the"))
            .ToList();

        Console.WriteLine("\nFiltered Loans (book title contains 'the'):");
        foreach (var loan in filteredLoans)
        {
            Console.WriteLine($"{loan.Member.Name} → {loan.Book.Title}");
        }
    }

    public void DisplayBooksByCategory()
    {
        var booksByCategory = _bookRepo.GetAllBooks()
            .GroupBy(b => b.Category.Name)
            .ToDictionary(g => g.Key, g => g.ToList());

        Console.WriteLine("\nBooks grouped by category:");
        foreach (var category in booksByCategory)
        {
            Console.WriteLine($"Category: {category.Key}");
            foreach (var book in category.Value)
            {
                Console.WriteLine($"  - {book.Title}");
            }
        }
    }

    public void DisplayUniqueCategories()
    {
        var categoriesList = _db.Categories
            .Select(c => c.Name)
            .Distinct()
            .ToList();

        Console.WriteLine("\nUnique categories:");
        foreach (var name in categoriesList)
        {
            Console.WriteLine($"- {name}");
        }
    }

    // ✅ New CRUD Methods

    public void UpdateBookTitle(int bookId, string newTitle)
    {
        var book = _db.Books.Find(bookId);
        if (book != null)
        {
            book.Title = newTitle;
            _db.SaveChanges();
            Console.WriteLine($"Book ID {bookId} title updated to: {newTitle}");
        }
        else
        {
            Console.WriteLine($"Book ID {bookId} not found.");
        }
    }

    public void DeleteLoan(int loanId)
    {
        var loan = _db.Loans.Find(loanId);
        if (loan != null)
        {
            _db.Loans.Remove(loan);
            _db.SaveChanges();
            Console.WriteLine($"Loan ID {loanId} deleted.");
        }
        else
        {
            Console.WriteLine($"Loan ID {loanId} not found.");
        }
    }

public void DisplayComplexFilteredLoans()
{
    var recentDate = DateTime.Now.AddDays(-10);

    var complexLoans = _db.Loans
        .Include(l => l.Book)
        .Include(l => l.Member)
        .Where(l =>
            l.Book.Title.ToLower().Contains("book") &&
            l.Member.Name.Contains("5") &&
            l.LoanDate >= recentDate
        )
        .ToList();

    Console.WriteLine("\n--- Complex Filtered Loans (title contains 'book', member contains '5', recent loans) ---");
    foreach (var loan in complexLoans)
    {
        Console.WriteLine($"{loan.Member.Name} borrowed '{loan.Book.Title}' on {loan.LoanDate.ToShortDateString()}");
    }
}

public void SearchBookByTitle(string keyword)
{
    var book = _bookRepo.GetAllBooks()
        .FirstOrDefault(b => b.Title.ToLower().Contains(keyword.ToLower()));

    Console.WriteLine($"\n--- Search Result for '{keyword}' ---");
    if (book != null)
        Console.WriteLine($"Found: {book.Title} by {book.Author}");
    else
        Console.WriteLine("No book found.");
}

public void FilterMembersByEmail()
{
    var filtered = _db.Members
        .Where(m => m.Contact.Email.EndsWith("@example.com"))
        .ToList();

    Console.WriteLine("\n--- Members with '@example.com' emails ---");
    foreach (var member in filtered)
    {
        Console.WriteLine($"{member.Name} - {member.Contact.Email}");
    }
}

public void SortBooksByYear()
{
    var sortedBooks = _bookRepo.GetAllBooks()
        .OrderByDescending(b => b.YearPublished)
        .ToList();

    Console.WriteLine("\n--- Books sorted by year (descending) ---");
    foreach (var book in sortedBooks)
    {
        Console.WriteLine($"{book.Title} - {book.YearPublished}");
    }
}


}
