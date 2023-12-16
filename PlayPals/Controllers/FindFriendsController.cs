using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayPals.Models;
using PlayPals.Responses;
using PlayPals.Services;

// Done by Ayman Tauhid
// Purpose of the mode: To find friends with similar genre and platform preferences
// Methods used: GetFriends, AddFriend

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindFriendsController : ControllerBase
    {
        private ApplicationDbContext _db;

        private readonly IConfiguration _configuration;

        public FindFriendsController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        // GET: /api/FindFriends/{email}
        [HttpGet("{email}")]
        // Find friends with similar genre and platform preferences
        public async Task<IActionResult> GetFriends(string email)
        {
            var user = await _db.Users.Include(u => u.Genres).Include(u => u.Platforms).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            var response = await _db.Users
                .Include(u => u.Genres)
                .Include(u => u.Platforms)
                .Where(u => u.UserId != user.UserId)
                .Select(u => new
                {
                    id = u.UserId,
                    email = u.Email,
                    genres = u.Genres.Select(g => new
                    {
                        id = g.Id,
                        name = g.Name
                    }),
                    platforms = u.Platforms.Select(p => new
                    {
                        id = p.Id,
                        name = p.Name
                    })
                })
                .OrderBy(u => u.email)
                .ToListAsync();

            var friends = new List<User>();
            foreach (var friend in response)
            {
                var genres = friend.genres?.Select(g => g.name).ToList();
                var platforms = friend.platforms?.Select(p => p.name).ToList();
                if (genres != null && platforms != null && genres.Intersect(user.Genres.Select(g => g.Name)).Any() && platforms.Intersect(user.Platforms.Select(p => p.Name)).Any())
                {
                    var userToAdd = await _db.Users.FindAsync(friend.id);
                    if (userToAdd != null)
                    {
                        friends.Add(userToAdd);
                    }
                }
            }

            return Ok(friends);
        }

        // POST: /api/FindFriends/{email}/addFriend
        [HttpPost("{email}/addFriend")]
        // Add friend to user's friend list
        public async Task<IActionResult> AddFriend(string email, string emailToAdd)
        {
            var user = await _db.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            var friend = await _db.Users.FirstOrDefaultAsync(u => u.Email == emailToAdd);
            if (friend == null)
            {
                return NotFound();
            }

            var friendToAdd = new Friend
            {
                FriendId = friend.UserId,
                User = friend
            };

            user.Friends.Add(friendToAdd);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Friend added successfully." });
        }
    }
}