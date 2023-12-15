using Microsoft.EntityFrameworkCore;
using PlayPals.Models;

namespace PlayPals.Services
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; } // Add this line
        public DbSet<Post> Posts{get; set;}


        // Add this constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        // You can remove or comment out the OnConfiguring method if you are configuring the DbContext in Program.cs
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite("Filename=applicationDb.db");
        // }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=applicationDb.db");
        }

    }
}
