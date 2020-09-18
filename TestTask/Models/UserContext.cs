using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class UserContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; } 
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
