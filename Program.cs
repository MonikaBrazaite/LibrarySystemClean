using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new LibraryContext())
        {
            // Add sample categories
            if (!db.Categories.Any())
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
                db.Categories.AddRange(categories);
                db.SaveChanges();
            }

            // Add sample members
            if (!db.Members.Any())
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

                db.Members.AddRange(members);
                db.SaveChanges();
            }

            // Add sample books
            if (!db.Books.Any())
            {
                var categories = db.Categories.Take(20).ToList();
                var books = Enumerable.Range(1, 20).Select(i => new Book
                {
                    Title = $"Book Title {i}",
                    Author = $"Author {i}",
                    ISBN = $"978-000000000{i:D2}",
                    YearPublished = 2000 + i,
                    CategoryId = categories[i % categories.Count].CategoryId
                }).ToList();

                db.Books.AddRange(books);
                db.SaveChanges();
            }

            // Add sample loans
            if (!db.Loans.Any())
            {
                var books = db.Books.Take(20).ToList();
                var members = db.Members.Take(20).ToList();
                var loans = Enumerable.Range(0, 20).Select(i => new Loan
                {
                    BookId = books[i % books.Count].BookId,
                    MemberId = members[i % members.Count].MemberId,
                    LoanDate = DateTime.Now.AddDays(-i)
                }).ToList();

                db.Loans.AddRange(loans);
                db.SaveChanges();
            }

            // Display all loans
            Console.WriteLine("Loans:");
            foreach (var loan in db.Loans
                .Include(l => l.Book)
                .Include(l => l.Member))
            {
                Console.WriteLine($"{loan.Member.Name} borrowed '{loan.Book.Title}' on {loan.LoanDate.ToShortDateString()}");
            }

            // Filter: loans where book title contains "the"
            var filteredLoans = db.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.Book.Title.ToLower().Contains("the"))
                .ToList();

            Console.WriteLine("\nFiltered Loans (book title contains 'the'):");
            foreach (var loan in filteredLoans)
            {
                Console.WriteLine($"{loan.Member.Name} → {loan.Book.Title}");
            }

            // Display all unique categories
            var categoriesList = db.Categories
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
}

