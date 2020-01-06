using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnleashedRage.Models;

namespace UnleashedRage.Database
{
    public class URContext : DbContext
    {
        public URContext(DbContextOptions<URContext> options)
            : base(options)
        {
        }

        public DbSet<ComicPage> ComicPage { get; set; }
        public DbSet<Merch> Merch { get; set; }
        public DbSet<User> User { get; set; }
    }
}
