using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class OwnerRepository : BaseRepository<OwnerEntity>, IOwnerRepository
    {
        public OwnerRepository(ISqliteWasmDbContextFactory<DataContext> factory) : base(factory) { }

        public async Task<int> DeleteRangeByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<OwnerEntity> owners = context.Owners.Where(o => o.Year == year);
            context.RemoveRange(owners);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllByYearWithAllPigeonsAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Year == year)
                .Include(o => o.Pigeons)
                .ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllByYearWithYearPigeonsAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Pigeons!.Any())
                .Where(o => o.Year == year)
                .Include(o => o.Pigeons!.Where(p => p.Year == year - 1))
                .ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllByYearWithYoungPigeonsAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Pigeons!.Any())
                .Where(o => o.Year == year)
                .Include(o => o.Pigeons!.Where(p => p.Year == year))
                .ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => ids.Contains(o.Id))
                .ToListAsync();
        }
    }
}
