using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface ISelectedYoungPigeonRepository : IBaseRepository<SelectedYoungPigeonEntity>
    {
        Task<int> DeleteByYearAndOwnerAsync(int year, int ownerId);
        Task<IEnumerable<SelectedYoungPigeonEntity>> GetAllByYearAsync(int year);
        Task<SelectedYoungPigeonEntity?> GetByYearAndOwnerAsync(int year, int ownerId);
        Task<SelectedYoungPigeonEntity?> GetByYearAndPigeonAsync(int year, int pigeonYear, string pigeonCountry, int pigeonRingNumber);
    }
}
