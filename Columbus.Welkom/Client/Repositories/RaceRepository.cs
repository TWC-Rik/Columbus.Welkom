using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ISqliteWasmDbContextFactory<DataContext> _factory;

        public RaceRepository(ISqliteWasmDbContextFactory<DataContext> factory)
        {
            _factory = factory;
        }

        public async Task AddRangeAsync(IEnumerable<RaceEntity> entities)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Races.AddRange(entities);
            await context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(r => r.StartTime.Year == year)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<RaceEntity>> GetAllByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(r => r.StartTime.Year == year)
                .Include(r => r.PigeonRaces)
                .ThenInclude(pr => pr.Pigeon)
                .ToListAsync();
        }

        public async Task AddRaceAsync(RaceEntity race)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Races.Add(race);
            await context.SaveChangesAsync();
        }
    }
}
