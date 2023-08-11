using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllByIdsAsync(IEnumerable<int> ids);
        Task<bool> DeleteRangeAsync(IEnumerable<T> entities);
        Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}