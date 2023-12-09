using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayPals.Models;
using PlayPals.Services;
using Microsoft.Extensions.Logging;

namespace PlayPals.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext db, ILogger<UserRepository> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            try
            {
                return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID: {UserId}", userId);
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", user.UserId);
                throw;
            }
        }
    }
}
