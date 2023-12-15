using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlayPals.Models
{
    public class Platform
    {
        [Key]
        public Guid Id { get; set;}
        public string Name { get; set; }
        public User? User { get; set; }
    }
}