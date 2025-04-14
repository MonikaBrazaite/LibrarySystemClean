public class LibraryFacade
{
    private readonly LibraryService _service;

    public LibraryFacade(LibraryService service)
    {
        _service = service;
    }

    public void InitializeSystem()
    {
        _service.RegisterObserver(new EmailNotifier());
        _service.RegisterObserver(new SMSNotifier()); // ✅ Add SMS notifications
        _service.SeedDatabase();
    }

    public void ShowAllLoans()
    {
        _service.DisplayAllLoans();
    }

    public void ShowFilteredLoans()
    {
        _service.DisplayFilteredLoans();
    }

    public void ShowBooksByCategory()
    {
        _service.DisplayBooksByCategory();
    }

    public void ShowUniqueCategories()
    {
        _service.DisplayUniqueCategories();
    }
 public void ChangeBookTitle(int bookId, string newTitle)
    {
        _service.UpdateBookTitle(bookId, newTitle);
    }

    public void RemoveLoan(int loanId)
    {
        _service.DeleteLoan(loanId);
    }

    public void ShowComplexFilteredLoans()
{
    _service.DisplayComplexFilteredLoans();
}

}
