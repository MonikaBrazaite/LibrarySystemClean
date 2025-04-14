using System;
using Microsoft.EntityFrameworkCore;

public class BasicLoanDisplayer : ILoanDisplayer
{
    private readonly LibraryContext _db;

    public BasicLoanDisplayer(LibraryContext db)
    {
        _db = db;
    }

    public void Display()
    {
        Console.WriteLine("Loans:");
        foreach (var loan in _db.Loans.Include(l => l.Book).Include(l => l.Member))
        {
            Console.WriteLine($"{loan.Member.Name} borrowed '{loan.Book.Title}' on {loan.LoanDate.ToShortDateString()}");
        }
    }
}
