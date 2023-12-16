using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayPals.Models;
using PlayPals.Services;
using PlayPals.DTOs;
using PlayPals.Responses;

// Done By Elias Alissandratos
// Purpose of the controller: To allow the user to create a post and view all posts.
// Methods used in the controller: Get, Post.

namespace PlayPals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public PostsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /api/posts
        [HttpGet]
        public async Task<IActionResult> GetAllPosts([FromQuery] int page)
        {
            var response = await _db.Posts
                .OrderByDescending(p => p.PostingDate)
                .Include(p => p.User)
                .Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Content = p.Content,
                    UserName = p.User != null ? p.User.Email : string.Empty,
                    PostingDate = p.PostingDate
                })
                .Skip((page - 1) * 10)
                .Take(10)
                .ToListAsync();

            var pagedResponse = new PagedResponse<PostDto>(response);
            var totalPosts = await _db.Posts.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalPosts / 10);

            pagedResponse.Links.Add("First", $"/api/posts?page=1");
            pagedResponse.Links.Add("Last", $"/api/posts?page={totalPages}");
            if (page < totalPages)
                pagedResponse.Links.Add("Next", $"/api/posts?page={page + 1}");
            if (page > 1)
                pagedResponse.Links.Add("Previous", $"/api/posts?page={page - 1}");

            pagedResponse.Meta.Add("totalPages", totalPages);
            pagedResponse.Meta.Add("totalPosts", totalPosts);

            return Ok(pagedResponse);
        }

        // GET /api/posts/{id}
        [HttpGet("/{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _db.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound("Post not found.");
            }

            var response = new
            {
                PostId = post.PostId,
                Content = post.Content,
                UserName = post.User?.Email,
                PostingDate = post.PostingDate
            };

            return Ok(response);
        }
    }
}
