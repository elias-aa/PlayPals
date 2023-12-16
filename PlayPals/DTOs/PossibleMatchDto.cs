using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Done by Ayman Tauhid
// Purpose of the mode: To store the possible match of the user in the database.

namespace PlayPals.Models
{
    public class PossibleMatchDto
    {
        public string Email { get; set; }
        public List<Genre>? Genres { get; set; }
        public List<Platform>? Platforms { get; set; }
    }
}