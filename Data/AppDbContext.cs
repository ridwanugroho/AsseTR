using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using AsseTS.Models;

namespace AsseTS.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Administrator> Admin { get; set; }
        public DbSet<Operator> Opeartor { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}