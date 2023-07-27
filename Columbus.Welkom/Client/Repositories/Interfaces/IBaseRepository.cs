namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task AddRangeAsync(IEnumerable<T> entities);
    }
}