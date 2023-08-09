using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class SelectedYoungPigeonRepository : BaseRepository<SelectedYoungPigeonEntity>, ISelectedYoungPigeonRepository
    {
        public SelectedYoungPigeonRepository(ISqliteWasmDbContextFactory<DataContext> factory): base(factory) { }

        public async Task<int> DeleteByYearAndOwnerAsync(int year, int ownerId)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<SelectedYoungPigeonEntity> selectedYoungPigeons = context.SelectedYoungPigeons.Where(syp => syp.Year == year)
                .Where(syp => syp.OwnerId == ownerId);
            context.RemoveRange(selectedYoungPigeons);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectedYoungPigeonEntity>> GetAllByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.SelectedYoungPigeons.Where(syp => syp.Year == year)
                .Include(syp => syp.Owner)
                .Include(syp => syp.Pigeon)
                .ToListAsync();
        }

        public async Task<SelectedYoungPigeonEntity?> GetByYearAndOwnerAsync(int year, int ownerId)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.SelectedYoungPigeons.Where(syp => syp.Year == year)
                .Where(syp => syp.OwnerId == ownerId)
                .FirstOrDefaultAsync();
        }

        public async Task<SelectedYoungPigeonEntity?> GetByYearAndPigeonAsync(int year, int pigeonYear, string pigeonCountry, int pigeonRingNumber)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.SelectedYoungPigeons.Where(syp => syp.Year == year)
                .Where(syp => syp.Pigeon!.Year == pigeonYear && syp.Pigeon.Country == pigeonCountry && syp.Pigeon.RingNumber == pigeonRingNumber)
                .Include(syp => syp.Pigeon)
                .Include(syp => syp.Owner)
                .FirstOrDefaultAsync();
        }
    }
}
