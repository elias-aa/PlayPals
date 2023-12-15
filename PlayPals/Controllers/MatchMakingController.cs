using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayPals.Models;
using PlayPals.Responses;
using PlayPals.Services;

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchMakingController : ControllerBase
    {
        private ApplicationDbContext _db;

        private readonly IConfiguration _configuration;

        public MatchMakingController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        // GET: /api/matchmaking/{email}
        [HttpGet("{email}")]
        public async Task<IActionResult> GetPossibleMatches(string email)
        {
            var user = await _db.Users
                .Include(u => u.Genres)
                .Include(u => u.Platforms)
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var possibleMatches = await _db.Users
                .Include(u => u.Genres)
                .Include(u => u.Platforms)
                .Where(u => u.Email != email)
                .Where(u => u.Genres.Any(g => user.Genres.Contains(g)))
                .Where(u => u.Platforms.Any(p => user.Platforms.Contains(p)))
                .Select(u => new PossibleMatchDto
                {
                    Email = u.Email,
                    Genres = u.Genres,
                    Platforms = u.Platforms
                })
                .ToListAsync();

            var response = new PagedResponse<PossibleMatchDto>(possibleMatches);
            response.Links.Add("Self", $"/api/matchmaking/{email}");

            return Ok(response);
        }
    }
}