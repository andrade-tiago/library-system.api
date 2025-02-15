using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public class LibraryDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<BookReservation> BookReservations { get; set; }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    { }
}

