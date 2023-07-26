using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class OwnerContext : DbContext
    {
        public OwnerContext(DbContextOptions<OwnerContext> opts) : base(opts) { }

        public DbSet<OwnerEntity> Owners { get; set; }
    }
}
