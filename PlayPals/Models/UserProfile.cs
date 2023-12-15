using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayPals.Models
{
    public class UserProfile
    {
        [Key]
        public Guid Id { get; set; }  // Primary key for UserProfile

        [ForeignKey("User")]
        public Guid UserId { get; set; } // Foreign key reference to User

        public string? ProfilePicturePath { get; set; }
        public string? Bio { get; set; }

        public virtual User User { get; set; } // Navigation property
    }
}
