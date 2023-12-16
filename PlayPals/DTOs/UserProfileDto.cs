using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Done by Fernanda Battig
// Purpose of the mode: To store the user profile in the database.

namespace PlayPals.DTOs
{
    public class UserProfileDto
    {
        public string Email { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? Bio { get; set; }
        public List<string>? Genres { get; set; }
        public List<string>? Platforms { get; set; }
    }
}