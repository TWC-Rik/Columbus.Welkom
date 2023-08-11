using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class OwnerRepository : BaseRepository<OwnerEntity>, IOwnerRepository
    {
        public OwnerRepository(ISqliteWasmDbContextFactory<DataContext> factory) : base(factory) { }

        public async Task<IEnumerable<OwnerEntity>> GetAllAsync()
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllWithAllPigeonsAsync()
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Include(o => o.Pigeons)
                .ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllWithYearPigeonsAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Pigeons!.Any())
                .Include(o => o.Pigeons!.Where(p => p.Year == year - 1))
                .ToListAsync();
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllWithYoungPigeonsAsync(int year)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Owners.Where(o => o.Pigeons!.Any())
                .Include(o => o.Pigeons!.Where(p => p.Year == year))
                .ToListAsync();
        }
    }
}
