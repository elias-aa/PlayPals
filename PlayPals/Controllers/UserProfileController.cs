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

        [HttpPost("uploadProfilePicture")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            var userProfile = await _db.UserProfiles.FindAsync(userId);

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (file.Length > 10 * 1024 * 1024) // 10 MB limit
                return BadRequest("File size exceeds limit.");

            var uploadsDirectory = Path.Combine("uploads", "profile_pictures");
            if (!Directory.Exists(uploadsDirectory))
                Directory.CreateDirectory(uploadsDirectory);

            var fileName = $"{userId}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (userProfile == null)
            {
                userProfile = new UserProfile
                {
                    UserId = userId,
                    ProfilePicturePath = filePath
                };
                _db.UserProfiles.Add(userProfile);
            }
            else
            {
                userProfile.ProfilePicturePath = filePath;
                _db.UserProfiles.Update(userProfile);
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Profile picture updated successfully." });
        }

        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserProfile>> GetProfile()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            var userProfile = await _db.UserProfiles.FindAsync(userId);
            if (userProfile == null) return NotFound("User profile not found.");

            return Ok(userProfile);
        }

        [HttpPost("updateBio")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateBio([FromBody] UpdateBioDto bioDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type is ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            var userProfile = await _db.UserProfiles.FindAsync(userId);
            if (userProfile == null) return NotFound("User profile not found.");

            userProfile.Bio = bioDto.NewBio;
            _db.UserProfiles.Update(userProfile);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Bio updated successfully." });
        }
    }
}
