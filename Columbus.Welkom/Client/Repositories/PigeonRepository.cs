using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
using System.Linq.Dynamic.Core;

namespace Columbus.Welkom.Client.Repositories
{
    public class PigeonRepository : IPigeonRepository
    {
        private readonly ISqliteWasmDbContextFactory<DataContext> _factory;

        public PigeonRepository(ISqliteWasmDbContextFactory<DataContext> factory)
        {
            _factory = factory;
        }

        public async Task AddRangeAsync(IEnumerable<PigeonEntity> pigeons)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.AddRange(pigeons);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PigeonEntity>> GetPigeonsByCountriesAndYearsAndRingNumbers(IEnumerable<Pigeon> pigeons)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<string> countries = pigeons.Select(p => p.Country).Distinct();
            IEnumerable<int> years = pigeons.Select(p => p.Year).Distinct();
            IEnumerable<int> ringNumbers = pigeons.Select(p => p.RingNumber).Distinct();

            IEnumerable<PigeonEntity> result = await context.Pigeons
                .Where(p => countries.Contains(p.Country))
                .Where(p => years.Contains(p.Year))
                .Where(p => ringNumbers.Contains(p.RingNumber))
                .ToListAsync();

            return result.Where(pe => pigeons.Any(p => p.Country == pe.Country && p.Year == pe.Year && p.RingNumber == pe.RingNumber));
        }
    }
}
