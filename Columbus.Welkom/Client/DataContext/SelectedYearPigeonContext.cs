using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class SelectedYearPigeonContext : DbContext
    {
        public SelectedYearPigeonContext(DbContextOptions<SelectedYearPigeonContext> opts) : base(opts) { }

        public DbSet<SelectedYearPigeonEntity> SelectedYearPigeons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SelectedYearPigeonEntity>().ToTable("selected_year_pigeon");
            modelBuilder.Entity<SelectedYearPigeonEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<SelectedYearPigeonEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<SelectedYearPigeonEntity>().HasOne(e => e.Owner);
            modelBuilder.Entity<SelectedYearPigeonEntity>().HasOne(e => e.Pigeon);
        }
    }
}
