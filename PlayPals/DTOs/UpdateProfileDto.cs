using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayPals.DTOs
{
    public class UpdateProfileDto
    {
        public string NewProfilePic { get; set; }
        public string NewBio { get; set; }
    }
}
