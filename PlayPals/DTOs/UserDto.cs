using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Done by Elias Aliisandratos
// Purpose of the mode: To store the post of the user in the database.

namespace PlayPals.DTOs
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}