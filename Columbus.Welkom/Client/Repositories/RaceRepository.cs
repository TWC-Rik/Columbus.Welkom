using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class RaceRepository : BaseRepository<RaceEntity>, IRaceRepository
    {
        public RaceRepository(ISqliteWasmDbContextFactory<DataContext> factory): base(factory) { }

        public async Task<int> DeleteRangeByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<RaceEntity> races = context.Races.Where(r => r.StartTime.Year == year);
            context.RemoveRange(races);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SimpleRaceEntity>> GetAllByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(r => r.StartTime.Year == year)
                .Select(r => new SimpleRaceEntity(r.Number, r.Type, r.Name, r.Code, r.StartTime, r.Latitude, r.Longitude, r.PigeonRaces!.Select(pr => pr.Pigeon!.Owner).Distinct().Count(), r.PigeonRaces!.Count()))
                .ToListAsync();
        }

        public async Task<IEnumerable<RaceEntity>> GetAllByYearAndTypes(int year, RaceType[] types)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(r => r.StartTime.Year == year)
                .Where(r => types.Contains(r.Type))
                .ToListAsync();
        }

        public async Task<IEnumerable<RaceEntity>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(o => ids.Contains(o.Id))
                .ToListAsync();
        }

        public async Task<RaceEntity> GetByCodeAndYear(string code, int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Races.Where(r => r.Code == code)
                .Where(r => r.StartTime.Year == year)
                .Include(r => r.PigeonRaces!)
                .ThenInclude(pr => pr.Pigeon!)
                .ThenInclude(p => p.Owner)
                .FirstAsync();
        }

        public async Task<bool> IsRaceCodePresentForYear(string code, int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Races.AnyAsync(r => r.Code == code);
        }

        public async Task<int> DeleteRaceByCodeAndYear(string code, int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<RaceEntity> races = context.Races.Where(r => r.Code == code && r.StartTime.Year == year);
            context.RemoveRange(races);
            return await context.SaveChangesAsync();
        }
    }
}
