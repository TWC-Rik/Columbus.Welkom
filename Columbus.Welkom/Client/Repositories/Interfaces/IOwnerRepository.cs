using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IOwnerRepository : IBaseRepository<OwnerEntity>
    {
        Task<IEnumerable<OwnerEntity>> GetAllAsync();
        Task<IEnumerable<OwnerEntity>> GetAllWithAllPigeonsAsync();
        Task<IEnumerable<OwnerEntity>> GetAllWithYearPigeonsAsync(int year, bool includeOwnersWithoutPigeons);
        Task<IEnumerable<OwnerEntity>> GetAllWithYoungPigeonsAsync(int year, bool includeOwnersWithoutPigeons);
    }
}
