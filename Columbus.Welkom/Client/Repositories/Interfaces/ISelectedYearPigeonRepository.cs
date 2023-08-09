using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface ISelectedYearPigeonRepository : IBaseRepository<SelectedYearPigeonEntity>
    {
        Task<int> DeleteByYearAndOwnerAsync(int year, int ownerId);
        Task<IEnumerable<SelectedYearPigeonEntity>> GetAllByYearAsync(int year);
        Task<SelectedYearPigeonEntity?> GetByYearAndOwnerAsync(int year, int ownerId);
        Task<SelectedYearPigeonEntity?> GetByYearAndPigeonAsync(int year, int pigeonYear, string pigeonCountry, int pigeonRingNumber);
    }
}
