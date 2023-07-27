using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class TeamContext : DbContext
    {
        public TeamContext(DbContextOptions<TeamContext> opts) : base(opts) { }

        public DbSet<TeamEntity> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeamEntity>().ToTable("team");
            modelBuilder.Entity<TeamEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<TeamEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<TeamEntity>().HasOne(e => e.FirstOwner);
            modelBuilder.Entity<TeamEntity>().HasOne(e => e.SecondOwner);
            modelBuilder.Entity<TeamEntity>().HasOne(e => e.ThirdOwner);
        }
    }
}
