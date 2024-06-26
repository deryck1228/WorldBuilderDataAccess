using Microsoft.EntityFrameworkCore;
using WorldBuilderDataAccessLib.Models;

namespace WorldBuilderDataAccessLib
{
    public class CampaignContext : DbContext
    {
        public CampaignContext(DbContextOptions<CampaignContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Character>().ToTable("Characters");
            modelBuilder.Entity<Location>().ToTable("Locations");
        }
    }
}
