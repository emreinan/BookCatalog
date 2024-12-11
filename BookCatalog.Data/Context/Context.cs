using BookCatalog.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
    }
}
