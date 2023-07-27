using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class PigeonContext : DbContext
    {
        public PigeonContext(DbContextOptions<PigeonContext> opts) : base(opts) { }

        public DbSet<PigeonEntity> Pigeons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PigeonEntity>().ToTable("pigeon");
            modelBuilder.Entity<PigeonEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<PigeonEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<PigeonEntity>().HasIndex(e => e.RingNumber);
            modelBuilder.Entity<PigeonEntity>().HasOne(e => e.Owner)
                .WithMany(e => e.Pigeons)
                .HasForeignKey(e => e.OwnerId);
        }
    }
}
