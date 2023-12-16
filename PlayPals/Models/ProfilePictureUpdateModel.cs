using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Done by Fernanda Battig
// Purpose of the mode: To store the profile picture of the user in the database.

namespace PlayPals.Models
{
    public class ProfilePictureUpdateModel
    {
        public string ProfilePicturePath { get; set; }
    }
}