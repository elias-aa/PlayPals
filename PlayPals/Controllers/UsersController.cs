
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using PlayPals.DTOs;
using PlayPals.Models;
using PlayPals.Services;
using Microsoft.Extensions.Configuration;

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private ApplicationDbContext _db;

        private readonly IConfiguration _configuration;

        public UsersController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        // POST /api/users/{userId}/post
        [HttpPost("{userId}/post")]
        public async Task<IActionResult> AddPostToUser(Guid userId, PostDto postDto)
        {
            var user = await _db.Users.Include(u => u.Posts).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var newPost = new Post
            {
                User = user,
                Content = postDto.Content,
                PostingDate = DateTime.Now
            };

            if (user.Posts != null)
            {
                user.Posts.Add(newPost);
            }
            else
            {
                user.Posts = new List<Post> { newPost };
            }
            await _db.SaveChangesAsync();

            var lastTenPosts = user.Posts.OrderByDescending(p => p.PostingDate).Take(10).Select(p => new PostDto
            {
                PostId = p.PostId,
                Content = p.Content,
                UserName = user.Email,
                PostingDate = p.PostingDate
            }).ToList();

            var response = new
            {
                Id = user.UserId,
                userEmail = user.Email,
                Posts = lastTenPosts
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _db.Users.ToListAsync();
        }

        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto userDto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return BadRequest("Email is already in use");
            }

            byte[] passwordHash, passwordSalt;
            new User().CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            User user = new User
            {
                Email = userDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserDto userDto)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (user == null)
            {
                return BadRequest("Email does not exist");
            }

            if (!new User().VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(Guid userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return user;
        }
    }
}

