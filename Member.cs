public class Member
{
    public int MemberId { get; set; }
    public string Name { get; set; } = string.Empty;

    // Composite object
    public ContactInfo Contact { get; set; } = new ContactInfo();

    // Extendable: can be used for roles (e.g., Admin, Student, etc.)
    public string Role { get; set; } = "Member";
}
