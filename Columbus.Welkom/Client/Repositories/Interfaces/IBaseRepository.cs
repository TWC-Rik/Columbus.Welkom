using Columbus.Models;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync();
    }
}