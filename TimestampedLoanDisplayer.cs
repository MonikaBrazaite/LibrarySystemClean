using System;

public class TimestampedLoanDisplayer : ILoanDisplayer
{
    private readonly ILoanDisplayer _inner;

    public TimestampedLoanDisplayer(ILoanDisplayer inner)
    {
        _inner = inner;
    }

    public void Display()
    {
        Console.WriteLine($"[Timestamp: {DateTime.Now}]");
        _inner.Display();
    }
}
