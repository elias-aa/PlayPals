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

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

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
    }
}