using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using EFCore.BulkExtensions;
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

            HashSet<int> ownerIds = owners.Select(o => o.Id).ToHashSet();

            context.BulkInsert(owners);
            BulkInsertWorkaround(context);
            await context.SaveChangesAsync();

            return owners;
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => ids.Contains(o.Id))
                .ToListAsync();
        }

        // SqliteWasmHelper does not work perfectly with EFCore.BulkExtensions
        // This is a minor workaround to force inserts.
        private void BulkInsertWorkaround(DataContext context)
        {
            var first = context.Owners.FirstOrDefault();
            if (first != null)
            {
                context.Entry(first).State = EntityState.Modified;
            }
        }
    }
}
