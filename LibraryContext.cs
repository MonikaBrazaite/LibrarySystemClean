using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=library.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Book → Category (many-to-one)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId);

        // Loan → Book (many-to-one)
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book)
            .WithMany()
            .HasForeignKey(l => l.BookId);

        // Loan → Member (many-to-one)
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Member)
            .WithMany()
            .HasForeignKey(l => l.MemberId);

        // Member → owns ContactInfo (composite object)
        modelBuilder.Entity<Member>()
            .OwnsOne(m => m.Contact);
    }
}
