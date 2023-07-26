using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class PigeonRaceContext : DbContext
    {
        public PigeonRaceContext(DbContextOptions<PigeonRaceContext> opts) : base(opts) { }

        public DbSet<PigeonRaceEntity> PigeonRaces { get; set; }
    }
}
