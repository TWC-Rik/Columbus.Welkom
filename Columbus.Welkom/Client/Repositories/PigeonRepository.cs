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

            IQueryable<PigeonEntity> query = context.Pigeons.Select(p => p);
            foreach (var p in pigeons)
                query = query.Union(context.Pigeons.Where(pe => pe.Country == p.Country && pe.Year == p.Year && pe.RingNumber == p.RingNumber));

            return await query.ToListAsync();
        }
    }
}
