using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayPals.Models;

namespace PlayPals.Repositories
{
     public interface IUserRepository
    {
    
        Task<User> GetUserByIdAsync(Guid userId); 
        Task UpdateUserAsync(User user);
    }
    
}