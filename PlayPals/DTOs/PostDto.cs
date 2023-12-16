using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Done by Elias Aliisandratos
// Purpose of the mode: To store the post of the user in the database.

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