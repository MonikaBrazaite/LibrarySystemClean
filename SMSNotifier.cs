using System;

public class SMSNotifier : ILoanObserver
{
    public void OnLoanCreated(Loan loan)
    {
        Console.WriteLine($"[SMS] Reminder: '{loan.Book?.Title}' has been loaned to {loan.Member?.Name} on {loan.LoanDate.ToShortDateString()}.");
    }
}
