using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayPals.DTOs;
using PlayPals.Models;
using PlayPals.Services;
using System;
using System.Threading.Tasks;

// Done By Fernanda Battig
// Purpose of the controller: To handle the user profile.
// Methods: UpdateProfilePicture, UpdateBio, GetUserProfile

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UserProfileController(ApplicationDbContext db)
        {
            _db = db;
        }

        // PUT /api/userProfile/{emailId}/uploadProfilePicture
        [HttpPut("{emailId}/uploadProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture(string emailId, [FromBody] ProfilePictureUpdateModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == emailId);
            if (user == null) return NotFound("User not found.");

            user.ProfilePicturePath = model.ProfilePicturePath;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Profile picture updated successfully." });
        }


        // PUT /api/userProfile/{emailId}/updateBio
        [HttpPut("{emailId}/updateBio")]
        public async Task<IActionResult> UpdateBio(string emailId, [FromBody] BioUpdateModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == emailId);
            if (user == null) return NotFound("User not found.");

            user.Bio = model.Bio;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Bio updated successfully." });
        }

        // GET /api/userProfile/{emailId}
        [HttpGet("{emailId}")]
        public async Task<IActionResult> GetUserProfile(string emailId)
        {
            var user = await _db.Users
                .Include(u => u.Genres)
                .Include(u => u.Platforms)
                .FirstOrDefaultAsync(u => u.Email == emailId);
            if (user == null) return NotFound("User not found.");

            return Ok(new UserProfileDto
            {
                Email = user.Email,
                Bio = user.Bio,
                ProfilePicturePath = user.ProfilePicturePath,
                Genres = user.Genres?.Select(g => g.Name).ToList(),
                Platforms = user.Platforms?.Select(p => p.Name).ToList()
            });
        }
    }
}
