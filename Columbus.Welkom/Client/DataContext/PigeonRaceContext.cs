using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class PigeonRaceContext : DbContext
    {
        public PigeonRaceContext(DbContextOptions<PigeonRaceContext> opts) : base(opts) { }

        public DbSet<PigeonRaceEntity> PigeonRaces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PigeonRaceEntity>().ToTable("pigeon_race");
            modelBuilder.Entity<PigeonRaceEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<PigeonRaceEntity>().HasIndex(e => e.Mark);
            modelBuilder.Entity<PigeonRaceEntity>().HasOne(e => e.Pigeon);
            modelBuilder.Entity<PigeonRaceEntity>().HasOne(e => e.Race)
                .WithMany(e => e.PigeonRaces)
                .HasForeignKey(e => e.RaceId);
        }
    }
}
