using Library.Domain;
using Microsoft.EntityFrameworkCore;

namespace Library.Storage;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")[
                "ConnectionString"]
            ?? throw new Exception("Connection string is not found."));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasKey(b => b.Id);
        modelBuilder.Entity<Book>().HasOne(b => b.Author).WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId);
        modelBuilder.Entity<Book>().HasOne(b => b.Genre).WithMany(a => a.Books)
            .HasForeignKey(b => b.GenreId);
        modelBuilder.Entity<Author>().HasKey(a => a.Id);
        modelBuilder.Entity<Genre>().HasKey(g => g.Id);
    }
}