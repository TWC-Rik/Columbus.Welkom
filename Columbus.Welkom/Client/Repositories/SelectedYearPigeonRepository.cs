using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class SelectedYearPigeonRepository : BaseRepository<SelectedYearPigeonEntity>, ISelectedYearPigeonRepository
    {
        public SelectedYearPigeonRepository(ISqliteWasmDbContextFactory<DataContext> factory): base(factory) { }

        public async Task<int> DeleteByYearAndOwnerAsync(int year, int ownerId)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<SelectedYearPigeonEntity> selectedYearPigeons = context.SelectedYearPigeons.Where(syp => syp.Year == year)
                .Where(syp => syp.OwnerId == ownerId);
            context.RemoveRange(selectedYearPigeons);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectedYearPigeonEntity>> GetAllByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.SelectedYearPigeons.Where(syp => syp.Year == year)
                .Include(syp => syp.Owner)
                .Include(syp => syp.Pigeon)
                .ToListAsync();
        }

        public async Task<SelectedYearPigeonEntity?> GetByYearAndOwnerAsync(int year, int ownerId)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.SelectedYearPigeons.Where(syp => syp.Year == year)
                .Where(syp => syp.OwnerId == ownerId)
                .FirstOrDefaultAsync();
        }

        public async Task<SelectedYearPigeonEntity?> GetByYearAndPigeonAsync(int year, int pigeonYear, string pigeonCountry, int pigeonRingNumber)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.SelectedYearPigeons.Where(syp => syp.Year == year)
                .Where(syp => syp.Pigeon!.Year == pigeonYear && syp.Pigeon.Country == pigeonCountry && syp.Pigeon.RingNumber == pigeonRingNumber)
                .Include(syp => syp.Pigeon)
                .Include(syp => syp.Owner)
                .FirstOrDefaultAsync();
        }
    }
}
