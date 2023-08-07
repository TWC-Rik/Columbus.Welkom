using Columbus.Models;
using Columbus.Welkom.Client.Repositories.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace Columbus.Welkom.Client.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ISqliteWasmDbContextFactory<DataContext> _factory;

        public BaseRepository(ISqliteWasmDbContextFactory<DataContext> factory)
        {
            _factory = factory;
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
