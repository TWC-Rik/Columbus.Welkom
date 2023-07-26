using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class TeamContext : DbContext
    {
        public TeamContext(DbContextOptions<TeamContext> opts) : base(opts) { }

        public DbSet<TeamEntity> Teams { get; set; }
    }
}
