using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class LibraryService
{
    private readonly LibraryContext _db;

    public LibraryService(LibraryContext db)
    {
        _db = db;
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


            _db.Books.AddRange(books);
            _db.SaveChanges();
        }

        if (!_db.Loans.Any())
        {
            var books = _db.Books.Take(20).ToList();
            var members = _db.Members.Take(20).ToList();
            var loans = Enumerable.Range(0, 20).Select(i => new Loan
            {
                BookId = books[i].BookId,
                MemberId = members[i].MemberId,
                LoanDate = DateTime.Now.AddDays(-i)
            }).ToList();

            _db.Loans.AddRange(loans);
            _db.SaveChanges();
        }
    }

    public void DisplayAllLoans()
    {
        Console.WriteLine("Loans:");
        foreach (var loan in _db.Loans
            .Include(l => l.Book)
            .Include(l => l.Member))
        {
            Console.WriteLine($"{loan.Member.Name} borrowed '{loan.Book.Title}' on {loan.LoanDate.ToShortDateString()}");
        }
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
        var booksByCategory = _db.Books
            .Include(b => b.Category)
            .ToList()
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
}
