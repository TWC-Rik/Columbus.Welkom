using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class PigeonSwapRepository : BaseRepository<PigeonSwapEntity>, IPigeonSwapRepository
    {
        public PigeonSwapRepository(ISqliteWasmDbContextFactory<DataContext> factory) : base(factory) { }

        public async Task<IEnumerable<PigeonSwapEntity>> GetAllByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.PigeonSwaps.Where(ps => ps.Year == year)
                .Include(ps => ps.Player)
                .Include(ps => ps.Owner)
                .Include(ps => ps.Pigeon)
                .Include(ps => ps.CoupledPlayer)
                .ToListAsync();
        }

        public async Task<PigeonSwapEntity?> GetByIdAsync(int id)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.PigeonSwaps.FirstAsync(ps => ps.Id == id);
        }

        public async Task<int> DeleteByYearAndPlayerAndPigeonAsync(int year, int playerId, string pigeonCountry, int pigeonYear, int pigeonRingNumber)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<PigeonSwapEntity> pigeonSwapEntities = context.PigeonSwaps.Where(ps => ps.Year == year)
                .Where(ps => ps.PlayerId == playerId)
                .Where(ps => ps.Pigeon!.Country == pigeonCountry && ps.Pigeon.Year == pigeonYear && ps.Pigeon.RingNumber == pigeonRingNumber);
            context.RemoveRange(pigeonSwapEntities);
            return await context.SaveChangesAsync();
        }
    }
}
