using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayPals.DTOs;
using PlayPals.Models;
using PlayPals.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private readonly IConfiguration _configuration;

        public UserProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("generateToken")]
        public ActionResult GenerateToken(UserDto userDto)
        {
            var secretKey = _configuration["JwtConfig:SecretKey"];
            Console.WriteLine($"Secret Key from Configuration: {secretKey}");

            var user = _db.Users.FirstOrDefault(u => u.Email == userDto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token = token });


        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),

            };

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // POST: api/Users/uploadProfilePicture
        [HttpPost("uploadProfilePicture")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (file.Length > 10 * 1024 * 1024) // 10 MB limit
            {
                return BadRequest("File size exceeds limit.");
            }

            var uploadsDirectory = Path.Combine("uploads", "profile_pictures");
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            var fileName = $"{userId}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.ProfilePicturePath = filePath;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Profile picture updated successfully." });
        }

        // GET: api/Users/profile
        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<User>> GetProfile()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }


            return Ok(new
            {
                user.Email,
                user.ProfilePicturePath,
                user.Bio

            });
        }

        // POST: api/Users/updateBio
        [HttpPost("updateBio")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateBio([FromBody] UpdateBioDto bioDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Bio = bioDto.NewBio;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Bio updated successfully." });
        }
    }
}