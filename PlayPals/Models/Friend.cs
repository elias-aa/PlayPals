using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Done by Ayman Tauhid
// Purpose of the mode: To store the friend of the user in the database.

namespace PlayPals.Models
{
    public class Friend
    {
        public Guid FriendId { get; set; }
        public User? User { get; set; }

    }
}