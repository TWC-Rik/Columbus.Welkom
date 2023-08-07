using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IOwnerRepository : IBaseRepository<OwnerEntity>
    {
        Task<int> DeleteRangeByYearAsync(int year);
        Task<IEnumerable<OwnerEntity>> GetAllByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<OwnerEntity>> GetAllByYearWithAllPigeonsAsync(int year);
        Task<IEnumerable<OwnerEntity>> GetAllByYearWithYearPigeonsAsync(int year);
        Task<IEnumerable<OwnerEntity>> GetAllByYearWithYoungPigeonsAsync(int year);
    }
}
