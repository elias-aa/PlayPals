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
using PlayPals.Repositories;
using PlayPals.Services;

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {

    private readonly ApplicationDbContext _db;
    private readonly IUserRepository _userRepository;

    public UsersController(ApplicationDbContext db, IUserRepository userRepository)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _db.Users.ToListAsync();
        }
        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto userDto)
        {
            // Check if email is already in use
            if (await _db.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return BadRequest("Email is already in use");
            }

            // Create password hash and salt
            byte[] passwordHash, passwordSalt;
            new User().CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            // Create new user
            User user = new User
            {
                Email = userDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            // Add user to database
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return Ok(user);
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserDto userDto)
        {
            // Check if email exists
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (user == null)
            {
                return BadRequest("Email does not exist");
            }

            // Check if password is correct
            if (!new User().VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect");
            }

            return Ok(user);
        }

   // PATCH: api/Users/updateProfile
    [HttpPatch("updateProfile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileDto updateProfileDto)
    {
        var userId = GetUserIdFromToken();
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found");
        }

        if (!string.IsNullOrEmpty(updateProfileDto.NewProfilePic))
        {
            user.ProfilePic = updateProfileDto.NewProfilePic;
        }

        if (!string.IsNullOrEmpty(updateProfileDto.NewBio))
        {
            user.Bio = updateProfileDto.NewBio;
        }

        await _userRepository.UpdateUserAsync(user);

        return Ok(new { message = "Profile updated successfully" });
    }


    private Guid GetUserIdFromToken()
    {
        // Implement logic to extract user ID from the token
        
        return Guid.Empty; 
    }
}

    
}