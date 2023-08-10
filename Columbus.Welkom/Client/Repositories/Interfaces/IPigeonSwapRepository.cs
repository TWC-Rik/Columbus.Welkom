using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IPigeonSwapRepository : IBaseRepository<PigeonSwapEntity>
    {
        Task<int> DeleteByYearAndPlayerAndPigeonAsync(int year, int playerId, string pigeonCountry, int pigeonYear, int pigeonRingNumber);
        Task<IEnumerable<PigeonSwapEntity>> GetAllByYearAsync(int year);
        Task<PigeonSwapEntity?> GetByIdAsync(int id);
    }
}
