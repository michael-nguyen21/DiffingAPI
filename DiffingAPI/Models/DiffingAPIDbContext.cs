using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DiffingAPI.Models
{
    public class DiffingAPIDbContext : DbContext
    {
        public DiffingAPIDbContext()
        {
        }

        public DbSet<Diff> Diff { get; set; }
    }
}