using Columbus.Models;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class PigeonContext : DbContext
    {
        public PigeonContext(DbContextOptions<PigeonContext> opts) : base(opts) { }

        public DbSet<Pigeon> Pigeons { get; set; }
    }
}
