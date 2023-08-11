using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Radzen;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class PigeonRaceRepository : BaseRepository<PigeonRaceEntity>, IPigeonRaceRepository
    {
        public PigeonRaceRepository(ISqliteWasmDbContextFactory<DataContext> factory): base(factory) { }

        public async Task DeleteAllByRaceId(int raceId)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<PigeonRaceEntity> pigeonRaces = context.PigeonRaces.Where(pr => pr.RaceId == raceId);
            context.RemoveRange(pigeonRaces);
            await context.SaveChangesAsync();
        }
    }
}
