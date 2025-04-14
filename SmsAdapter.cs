
public class SmsAdapter : INotifier
{
    private readonly SmsService _smsService;
    private readonly string _defaultNumber = "+37060000000"; // Simulated recipient

    public SmsAdapter(SmsService smsService)
    {
        _smsService = smsService;
    }

    public void Notify(string message)
    {
        _smsService.SendText(_defaultNumber, message);
    }
}
