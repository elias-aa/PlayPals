using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PlayPals.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        //Create Password Hash and Salt
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //Create Hashing Algorithm
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //Set Salt
                passwordSalt = hmac.Key;
                //Set Hash
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //Verify Password Hash
        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            //Create Hashing Algorithm
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                //Compute Hash
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                //Compare Hashes
                for (int i = 0; i < computedHash.Length; i++)
                {
                    //If Hashes Don't Match
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
                //If Hashes Match
                return true;
            }
        }

    }
}