using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayPals.Models;
using PlayPals.Services;

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private ApplicationDbContext _db;

        private readonly IConfiguration _configuration;

        public PreferencesController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        // GET: /api/preferences/
        [HttpGet]
        public async Task<IActionResult> GetAllPreferences()
        {
            var response = await _db.Users
                .Include(u => u.Genres)
                .Include(u => u.Platforms)
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

            return Ok(response);
        }

        // POST: /api/preferences/{email}/genres
        [HttpPost("{email}/genres")]
        public async Task<IActionResult> AddGenre(string email, Genre genre)
        {
            var user = await _db.Users.Include(u => u.Genres).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var newGenre = new Genre
            {
                Name = genre.Name,
                User = user
            };

            if (user.Genres != null)
            {
                user.Genres.Add(newGenre);
            }
            else
            {
                user.Genres = new List<Genre> { newGenre };
            }

            await _db.SaveChangesAsync();

            var response = new
            {
                id = user.UserId,
                email = user.Email,
                genres = user.Genres.Select(g => new
                {
                    id = g.Id,
                    name = g.Name
                })
            };

            return Ok(response);
        }

        // POST: /api/preferences/{email}/platforms
        [HttpPost("{email}/platforms")]
        public async Task<IActionResult> AddPlatform(string email, Platform platform)
        {
            var user = await _db.Users.Include(u => u.Platforms).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var newPlatform = new Platform
            {
                Name = platform.Name,
                User = user
            };

            if (user.Platforms != null)
            {
                user.Platforms.Add(newPlatform);
            }
            else
            {
                user.Platforms = new List<Platform> { newPlatform };
            }

            await _db.SaveChangesAsync();

            var response = new
            {
                id = user.UserId,
                email = user.Email,
                platforms = user.Platforms.Select(p => new
                {
                    id = p.Id,
                    name = p.Name
                })
            };

            return Ok(response);
        }
    }
}