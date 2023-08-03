using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ISqliteWasmDbContextFactory<DataContext> _factory;

        public OwnerRepository(ISqliteWasmDbContextFactory<DataContext> factory)
        {
            _factory = factory;
        }

        public async Task<int> DeleteRangeByYearAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Year == year)
                .ExecuteDeleteAsync();
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

            return await context.Owners.Where(o => o.Year == year)
                .Include(o => o.Pigeons!.Where(p => p.Year == year - 1))
                .ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllByYearWithYoungPigeonsAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Year == year)
                .Include(o => o.Pigeons!.Where(p => p.Year == year - 1))
                .ToListAsync();
        }

        public async Task<OwnerEntity> AddAsync(OwnerEntity owner)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Owners.Add(owner);
            await context.SaveChangesAsync();

            return owner;
        }

        public async Task<IEnumerable<OwnerEntity>> AddRangeAsync(IEnumerable<OwnerEntity> owners)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Owners.AddRange(owners);
            await context.SaveChangesAsync();

            return owners;
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => ids.Contains(o.Id))
                .ToListAsync();
        }
    }
}
