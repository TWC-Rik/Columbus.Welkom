using Columbus.Welkom.Client.DataContext;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ISqliteWasmDbContextFactory<OwnerContext> _factory;

        public OwnerRepository(ISqliteWasmDbContextFactory<OwnerContext> factory)
        {
            _factory = factory;
        }

        public async Task<int> DeleteRangeByYearAsync(int year)
        {
            using OwnerContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Year == year)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllByYearAsync(int year)
        {
            using OwnerContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Year == year)
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<OwnerEntity> owners)
        {
            using OwnerContext context = await _factory.CreateDbContextAsync();

            await context.Owners.AddRangeAsync(owners);
            await context.SaveChangesAsync();
        }
    }
}
