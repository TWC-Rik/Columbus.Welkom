using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class PigeonSwapContext : DbContext
    {
        public PigeonSwapContext(DbContextOptions<PigeonSwapContext> opts) : base(opts) { }

        public DbSet<PigeonSwapEntity> PigeonSwaps { get; set; }
    }
}
