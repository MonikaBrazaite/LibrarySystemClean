using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        using (var db = new LibraryContext())
        {
            // Add sample categories
            if (!db.Categories.Any())
            {
                var fiction = new Category { Name = "Fiction" };
                var sciFi = new Category { Name = "Science Fiction" };
                var fantasy = new Category { Name = "Fantasy" };

                db.Categories.AddRange(fiction, sciFi, fantasy);
                db.SaveChanges();
            }

            // Add sample members
            if (!db.Members.Any())
            {
                var member1 = new Member
                {
                    Name = "Alice Johnson",
                    Contact = new ContactInfo
                    {
                        Email = "alice@example.com",
                        PhoneNumber = "123-456-7890"
                    }
                };

                var member2 = new Member
                {
                    Name = "Bob Smith",
                    Contact = new ContactInfo
                    {
                        Email = "bob@example.com",
                        PhoneNumber = "987-654-3210"
                    }
                };

                db.Members.AddRange(member1, member2);
                db.SaveChanges();
            }

            // Add sample books
            if (!db.Books.Any())
            {
                var fictionCategory = db.Categories.FirstOrDefault(c => c.Name == "Fiction");
                var fantasyCategory = db.Categories.FirstOrDefault(c => c.Name == "Fantasy");

                var book1 = new Book
                {
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    ISBN = "978-0547928227",
                    YearPublished = 1937,
                    CategoryId = fantasyCategory.CategoryId
                };

                var book2 = new Book
                {
                    Title = "1984",
                    Author = "George Orwell",
                    ISBN = "978-0451524935",
                    YearPublished = 1949,
                    CategoryId = fictionCategory.CategoryId
                };

                db.Books.AddRange(book1, book2);
                db.SaveChanges();
            }

            // Add a sample loan
            if (!db.Loans.Any())
            {
                var firstBook = db.Books.First();
                var firstMember = db.Members.First();

                var loan = new Loan
                {
                    BookId = firstBook.BookId,
                    MemberId = firstMember.MemberId,
                    LoanDate = DateTime.Now
                };

                db.Loans.Add(loan);
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

            // Load all loans into a List
            List<Loan> allLoans = db.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToList();

            // Filter: loans where book title contains "the"
            var filteredLoans = allLoans
                .Where(l => l.Book.Title.ToLower().Contains("the"))
                .ToList();

            // Display filtered results
            Console.WriteLine("\nFiltered Loans (book title contains 'the'):");
            foreach (var loan in filteredLoans)
            {
                Console.WriteLine($"{loan.Member.Name} → {loan.Book.Title}");
            }

            // Bonus: List all distinct categories
            Console.WriteLine("\nUnique categories:");
            foreach (var category in db.Categories)
            {
                Console.WriteLine($"- {category.Name}");
            }
        }
    }
}
