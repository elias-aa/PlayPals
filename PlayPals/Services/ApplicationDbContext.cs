using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayPals.Models;
using Microsoft.Extensions.Logging;

namespace PlayPals.Services
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        private readonly ILogger<ApplicationDbContext> _logger;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsyncWithTransaction()
        {
            using var transaction = await Database.BeginTransactionAsync();
            try
            {
                int result = await SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred during Save Changes with Transaction.");
                throw;
            }
        }
    }
}
