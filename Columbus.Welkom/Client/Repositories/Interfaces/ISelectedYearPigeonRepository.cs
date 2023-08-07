using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface ISelectedYearPigeonRepository : IBaseRepository<SelectedYearPigeonEntity>
    {
        Task<IEnumerable<SelectedYearPigeonEntity>> GetAllByYearAsync(int year);
        Task<SelectedYearPigeonEntity?> GetByYearAndOwnerAsync(int year, int ownerId);
    }
}
