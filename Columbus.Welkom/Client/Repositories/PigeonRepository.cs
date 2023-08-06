using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using EFCore.BulkExtensions;
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

        public async Task<PigeonEntity> AddAsync(PigeonEntity pigeon)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Add(pigeon);
            await context.SaveChangesAsync();

            return pigeon;
        }

        public async Task<IEnumerable<PigeonEntity>> AddRangeAsync(IEnumerable<PigeonEntity> pigeons)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.BulkInsert(pigeons);
            BulkInsertWorkaround(context);
            await context.SaveChangesAsync();

            return pigeons;
        }

        public async Task<IEnumerable<PigeonEntity>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Pigeons.Where(o => ids.Contains(o.Id))
                .ToListAsync();
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

        // SqliteWasmHelper does not work perfectly with EFCore.BulkExtensions
        // This is a minor workaround to force inserts.
        private void BulkInsertWorkaround(DataContext context)
        {
            var first = context.Pigeons.FirstOrDefault();
            if (first != null)
            {
                context.Entry(first).State = EntityState.Modified;
            }
        }
    }
}
