public static class MemberFactory
{
    public static Member Create(string name, string email, string phone)
    {
        return new Member
        {
            Name = name,
            Contact = new ContactInfo { Email = email, PhoneNumber = phone }
        };
    }
}
