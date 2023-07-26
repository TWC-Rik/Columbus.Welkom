using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class SelectedYearPigeonContext : DbContext
    {
        public SelectedYearPigeonContext(DbContextOptions<SelectedYearPigeonContext> opts) : base(opts) { }

        public DbSet<SelectedYearPigeonEntity> SelectedYearPigeons { get; set; }
    }
}
