using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IOwnerRepository : IBaseRepository<OwnerEntity>
    {
        Task<int> DeleteRangeByYearAsync(int year);
        Task<IEnumerable<OwnerEntity>> GetAllByYearAsync(int year);
    }
}
