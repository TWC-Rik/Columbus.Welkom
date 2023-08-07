using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
using System.Linq.Dynamic.Core;

namespace Columbus.Welkom.Client.Repositories
{
    public class PigeonRepository : BaseRepository<PigeonEntity>, IPigeonRepository
    {
        public PigeonRepository(ISqliteWasmDbContextFactory<DataContext> factory): base(factory) { }

        public async Task<IEnumerable<PigeonEntity>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Pigeons.Where(o => ids.Contains(o.Id))
                .ToListAsync();
        }

        public async Task<PigeonEntity> GetByCountryAndYearAndRingNumberAsync(string country, int year, int ringNumber)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Pigeons.Where(p => p.Country == country)
                .Where(p => p.Year == year)
                .Where(p => p.RingNumber == ringNumber)
                .FirstAsync();
        }

        public async Task<IEnumerable<PigeonEntity>> GetPigeonsByCountriesAndYearsAndRingNumbersAsync(IEnumerable<Pigeon> pigeons)
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
