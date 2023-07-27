using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class RaceContext : DbContext
    {
        public RaceContext(DbContextOptions<RaceContext> opts) : base(opts) { }

        public DbSet<RaceEntity> Races { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RaceEntity>().ToTable("race");
            modelBuilder.Entity<RaceEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<RaceEntity>().HasMany(e => e.PigeonRaces)
                .WithOne(e => e.Race)
                .HasForeignKey(e => e.RaceId);
        }
    }
}
