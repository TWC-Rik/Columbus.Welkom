using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class PigeonSwapContext : DbContext
    {
        public PigeonSwapContext(DbContextOptions<PigeonSwapContext> opts) : base(opts) { }

        public DbSet<PigeonSwapEntity> PigeonSwaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PigeonSwapEntity>().ToTable("pigeon_swap");
            modelBuilder.Entity<PigeonSwapEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<PigeonSwapEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<PigeonSwapEntity>().HasOne(e => e.Player);
            modelBuilder.Entity<PigeonSwapEntity>().HasOne(e => e.Owner);
            modelBuilder.Entity<PigeonSwapEntity>().HasOne(e => e.CoupledPlayer);
        }
    }
}
