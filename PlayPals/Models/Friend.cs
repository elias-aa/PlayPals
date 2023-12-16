using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayPals.Models
{
    public class Friend
    {
        public Guid FriendId { get; set; }
        public User? User { get; set; }

    }
}