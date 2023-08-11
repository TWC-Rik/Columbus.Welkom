using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Radzen;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ISqliteWasmDbContextFactory<DataContext> _factory;

        public BaseRepository(ISqliteWasmDbContextFactory<DataContext> factory)
        {
            _factory = factory;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            return await context.Set<T>()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

             return await context.Set<T>()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Add(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.BulkInsert(entities);
            BulkInsertWorkaround(context);
            await context.SaveChangesAsync();

            return entities;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            context.Entry(entity).State = EntityState.Deleted;
            int count = await context.SaveChangesAsync();

            return count == 1;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            T entity = context.Set<T>().First(e => e.Id == id);

            return await DeleteAsync(entity);
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            foreach (T entity in entities)
                context.Entry(entity).State = EntityState.Deleted;

            int count = await context.SaveChangesAsync();
            return count == entities.Count();
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
        {
            using DataContext context = await _factory.CreateDbContextAsync();

            IEnumerable<T> entities = context.Set<T>().Where(e => ids.Contains(e.Id));

            return await DeleteRangeAsync(entities);
        }

        // SqliteWasmHelper does not work perfectly with EFCore.BulkExtensions
        // This is a minor workaround to force inserts.
        protected void BulkInsertWorkaround(DataContext context)
        {
            var first = context.Pigeons.FirstOrDefault();
            if (first != null)
            {
                context.Entry(first).State = EntityState.Modified;
            }
        }
    }
}
