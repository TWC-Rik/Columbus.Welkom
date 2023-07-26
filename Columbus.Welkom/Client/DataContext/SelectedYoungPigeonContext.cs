using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class SelectedYoungPigeonContext : DbContext
    {
        public SelectedYoungPigeonContext(DbContextOptions<SelectedYoungPigeonContext> opts) : base(opts) { }

        public DbSet<SelectedYoungPigeonEntity> SelectedYoungPigeons { get; set; }
    }
}
