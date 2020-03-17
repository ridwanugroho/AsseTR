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
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<History> Histories { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}