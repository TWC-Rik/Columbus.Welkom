using Columbus.Welkom.Client.DataContext;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ISqliteWasmDbContextFactory<RaceContext> _factory;

        public RaceRepository(ISqliteWasmDbContextFactory<RaceContext> factory)
        {
            _factory = factory;
        }

        public Task AddRangeAsync(IEnumerable<RaceEntity> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteRangeByYearAsync(int year)
        {
            using RaceContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(r => r.StartTime.Year == year)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<RaceEntity>> GetAllByYearAsync(int year)
        {
            using RaceContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(r => r.StartTime.Year == year)
                .Include(r => r.PigeonRaces)
                .ToListAsync();
        }
    }
}
