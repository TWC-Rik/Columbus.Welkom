using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class RaceContext : DbContext
    {
        public RaceContext(DbContextOptions<RaceContext> opts) : base(opts) { }

        public DbSet<RaceEntity> Races { get; set; }
    }
}
