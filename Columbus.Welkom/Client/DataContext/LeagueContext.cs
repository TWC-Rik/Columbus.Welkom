using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class LeagueContext : DbContext
    {
        public LeagueContext(DbContextOptions<LeagueContext> opts) : base(opts) { }

        public DbSet<LeagueEntity> Leagues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeagueEntity>().ToTable("league");
            modelBuilder.Entity<LeagueEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<LeagueEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<LeagueEntity>().HasOne(e => e.Owner);
        }
    }
}
