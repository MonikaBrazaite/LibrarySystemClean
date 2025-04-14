
public class SmsService
{
    public void SendText(string number, string text)
    {
        Console.WriteLine($"[SMS to {number}]: {text}");
    }
}
