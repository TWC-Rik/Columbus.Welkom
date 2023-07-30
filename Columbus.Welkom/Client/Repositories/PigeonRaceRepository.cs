using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Radzen;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class PigeonRaceRepository : IPigeonRaceRepository
    {
        private readonly ISqliteWasmDbContextFactory<DataContext> _factory;

        public PigeonRaceRepository(ISqliteWasmDbContextFactory<DataContext> factory)
        {
            _factory = factory;
        }

        public async Task<PigeonRaceEntity> AddAsync(PigeonRaceEntity entity)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.PigeonRaces.Add(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<PigeonRaceEntity>> AddRangeAsync(IEnumerable<PigeonRaceEntity> entities)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.PigeonRaces.AddRange(entities);
            await context.SaveChangesAsync();

            return entities;
        }

        public async Task<IEnumerable<PigeonRaceEntity>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.PigeonRaces.Where(pr => ids.Contains(pr.Id))
                .ToListAsync();

        }
    }
}
