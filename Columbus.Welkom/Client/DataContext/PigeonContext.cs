using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class PigeonContext : DbContext
    {
        public PigeonContext(DbContextOptions<PigeonContext> opts) : base(opts) { }

        public DbSet<PigeonEntity> Pigeons { get; set; }
    }
}
