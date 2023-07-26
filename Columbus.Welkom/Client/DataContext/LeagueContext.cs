using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class LeagueContext : DbContext
    {
        public LeagueContext(DbContextOptions<LeagueContext> opts) : base(opts) { }

        public DbSet<LeagueEntity> Leagues { get; set; }
    }
}
