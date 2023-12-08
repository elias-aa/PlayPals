using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayPals.Models;

namespace PlayPals.Services
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users{get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=applicationDb.db");
        }

    }
}