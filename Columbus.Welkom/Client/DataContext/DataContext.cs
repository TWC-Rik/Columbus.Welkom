using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client
{
    // Unfortunately WASM-SQLite will attempt to overwrite the database file in cache if another context tries to access the file.
    // This throws a big exception and means that a single context is necessary.
    // An unwieldy file like this is the result.
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

        public DbSet<PigeonEntity> Pigeons { get; set; }

        public DbSet<OwnerEntity> Owners { get; set; }

        public DbSet<RaceEntity> Races { get; set; }

        public DbSet<PigeonRaceEntity> PigeonRaces { get; set; }

        public DbSet<SelectedYoungPigeonEntity> SelectedYoungPigeons { get; set; }

        public DbSet<SelectedYearPigeonEntity> SelectedYearPigeons { get; set; }

        public DbSet<LeagueEntity> Leagues { get; set; }

        public DbSet<TeamEntity> Teams { get; set; }

        public DbSet<PigeonSwapEntity> PigeonSwaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pigeon
            modelBuilder.Entity<PigeonEntity>().ToTable("pigeon");
            modelBuilder.Entity<PigeonEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<PigeonEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<PigeonEntity>().HasIndex(e => e.RingNumber);
            modelBuilder.Entity<PigeonEntity>().HasOne(e => e.Owner)
                .WithMany(e => e.Pigeons)
                .HasForeignKey(e => e.OwnerId);

            // Owner
            modelBuilder.Entity<OwnerEntity>().ToTable("owner");
            modelBuilder.Entity<OwnerEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<OwnerEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<OwnerEntity>().HasMany(e => e.Pigeons)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerId);

            // Race
            modelBuilder.Entity<RaceEntity>().ToTable("race");
            modelBuilder.Entity<RaceEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<RaceEntity>().HasMany(e => e.PigeonRaces)
                .WithOne(e => e.Race)
                .HasForeignKey(e => e.RaceId);

            // PigeonRace
            modelBuilder.Entity<PigeonRaceEntity>().ToTable("pigeon_race");
            modelBuilder.Entity<PigeonRaceEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<PigeonRaceEntity>().HasIndex(e => e.Mark);
            modelBuilder.Entity<PigeonRaceEntity>().HasOne(e => e.Pigeon)
                .WithMany(e => e.PigeonRaces)
                .HasForeignKey(e => e.PigeonId);
            modelBuilder.Entity<PigeonRaceEntity>().HasOne(e => e.Race)
                .WithMany(e => e.PigeonRaces)
                .HasForeignKey(e => e.RaceId);

            // SelectedYoungPigeon
            modelBuilder.Entity<SelectedYoungPigeonEntity>().ToTable("selected_young_pigeon");
            modelBuilder.Entity<SelectedYoungPigeonEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<SelectedYoungPigeonEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<SelectedYoungPigeonEntity>().HasOne(e => e.Owner);
            modelBuilder.Entity<SelectedYoungPigeonEntity>().HasOne(e => e.Pigeon);

            // SelectedYearPigeon
            modelBuilder.Entity<SelectedYearPigeonEntity>().ToTable("selected_year_pigeon");
            modelBuilder.Entity<SelectedYearPigeonEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<SelectedYearPigeonEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<SelectedYearPigeonEntity>().HasOne(e => e.Owner);
            modelBuilder.Entity<SelectedYearPigeonEntity>().HasOne(e => e.Pigeon);

            // League
            modelBuilder.Entity<LeagueEntity>().ToTable("league");
            modelBuilder.Entity<LeagueEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<LeagueEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<LeagueEntity>().HasOne(e => e.Owner);

            // Team
            modelBuilder.Entity<TeamEntity>().ToTable("team");
            modelBuilder.Entity<TeamEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<TeamEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<TeamEntity>().HasOne(e => e.FirstOwner);
            modelBuilder.Entity<TeamEntity>().HasOne(e => e.SecondOwner);
            modelBuilder.Entity<TeamEntity>().HasOne(e => e.ThirdOwner);

            // PigeonSwap
            modelBuilder.Entity<PigeonSwapEntity>().ToTable("pigeon_swap");
            modelBuilder.Entity<PigeonSwapEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<PigeonSwapEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<PigeonSwapEntity>().HasOne(e => e.Player);
            modelBuilder.Entity<PigeonSwapEntity>().HasOne(e => e.Owner);
            modelBuilder.Entity<PigeonSwapEntity>().HasOne(e => e.CoupledPlayer);
        }
    }
}
