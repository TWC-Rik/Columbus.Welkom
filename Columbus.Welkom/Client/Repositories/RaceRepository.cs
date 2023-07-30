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

        public async Task<RaceEntity> AddAsync(RaceEntity race)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Races.Add(race);
            await context.SaveChangesAsync();

            return race;
        }

        public async Task<IEnumerable<RaceEntity>> AddRangeAsync(IEnumerable<RaceEntity> races)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Races.AddRange(races);
            await context.SaveChangesAsync();

            return races;
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
                .Include(r => r.PigeonRaces!)
                .ThenInclude(pr => pr.Pigeon)
                .ToListAsync();
        }

        public async Task<IEnumerable<RaceEntity>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(o => ids.Contains(o.Id))
                .ToListAsync();
        }
    }
}
