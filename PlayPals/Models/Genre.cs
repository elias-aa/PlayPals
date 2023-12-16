using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// Done by Ayman Tauhid
// Purpose of the mode: To store the genre of the user in the database.

namespace PlayPals.Models
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User? User { get; set; }
        
    }
}