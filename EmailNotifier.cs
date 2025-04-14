using System;

public class EmailNotifier : ILoanObserver
{
    public void OnLoanCreated(Loan loan)
    {
        Console.WriteLine($"[EMAIL] Notification: '{loan.Book.Title}' loaned to {loan.Member.Name} on {loan.LoanDate.ToShortDateString()}.");
    }
}
