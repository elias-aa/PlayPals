using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayPals.Models
{
    public class PossibleMatchDto
    {
        public string Email { get; set; }
        public List<Genre>? Genres { get; set; }
        public List<Platform>? Platforms { get; set; }
    }
}