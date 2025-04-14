public interface ICategoryFormatter
{
    string Format(string name);
}

public class CategoryNameAdapter : ICategoryFormatter
{
    public string Format(string name) => name.ToUpperInvariant();
}
