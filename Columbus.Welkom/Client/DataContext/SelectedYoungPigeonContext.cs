using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class SelectedYoungPigeonContext : DbContext
    {
        public SelectedYoungPigeonContext(DbContextOptions<SelectedYoungPigeonContext> opts) : base(opts) { }

        public DbSet<SelectedYoungPigeonEntity> SelectedYoungPigeons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SelectedYoungPigeonEntity>().ToTable("selected_young_pigeon");
            modelBuilder.Entity<SelectedYoungPigeonEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<SelectedYoungPigeonEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<SelectedYoungPigeonEntity>().HasOne(e => e.Owner);
            modelBuilder.Entity<SelectedYoungPigeonEntity>().HasOne(e => e.Pigeon);
        }
    }
}
