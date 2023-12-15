using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayPals.Models
{
    public class PostDto
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string? UserName { get; set; }
        public DateTime PostingDate { get; set; }
    }
}