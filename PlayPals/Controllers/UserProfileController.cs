using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayPals.DTOs;
using PlayPals.Models;
using PlayPals.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;

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

        // PUT /api/userProfile/uploadProfilePicture
        [HttpPut("{emailId}/uploadProfilePicture")]
        public async Task<IActionResult> AddImageToUser(string emailId, [FromBody]string imageUrl)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == emailId);
            if (user == null) return NotFound("User not found.");

            // user.ProfilePicturePath = imageUrl;
            // await _db.SaveChangesAsync();

            return Ok(user);
        }

        // PUT /api/userProfile/updateBio
        // Update user bio
        [HttpPut("{emailId}/updateBio")]
        public async Task<IActionResult> UpdateBio(string emailId, string bio)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == emailId);
            if (user == null) return NotFound("User not found.");

            user.Bio = bio;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Bio updated successfully." });
        }

        // GET /api/userProfile/{emailId}
        [HttpGet("{emailId}")]
        public async Task<IActionResult> GetUserProfile(string emailId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == emailId);
            if (user == null) return NotFound("User not found.");

            var response = new UserProfileDto
            {
                Email = user.Email,
                ProfilePicturePath = user.ProfilePicturePath,
                Bio = user.Bio
            };

            return Ok(response);
        }
    }
}
