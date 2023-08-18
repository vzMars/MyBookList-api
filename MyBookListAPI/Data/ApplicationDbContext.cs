using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBookListAPI.Models;

namespace MyBookListAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookUser> BookUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>()
                .HasMany(e => e.Authors)
                .WithMany(e => e.Books)
                .UsingEntity<BookAuthor>();
            modelBuilder.Entity<Book>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Books)
                .UsingEntity<BookUser>();
        }
    }
}
