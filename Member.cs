public class Member
{
    public int MemberId { get; set; }
    public string Name { get; set; } = string.Empty;

    // Composite object
    public ContactInfo Contact { get; set; } = new ContactInfo();
}
