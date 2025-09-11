using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TMG_Site_API.Models;


namespace TMG_Site_API.Data
{
    public class TmgPoolApiContext : DbContext
    {
        public TmgPoolApiContext (DbContextOptions<TmgPoolApiContext> options)
            : base(options)
        {
        }

        public DbSet<TmgPrice> TmgPrices { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TmgPrice>().HasKey(x => x.Epoch);
        }

    }
}
