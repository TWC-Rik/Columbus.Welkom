using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IPigeonRepository : IBaseRepository<PigeonEntity>
    {
        Task<PigeonEntity> GetByCountryAndYearAndRingNumberAsync(string country, int year, int ringNumber);
        Task<IEnumerable<PigeonEntity>> GetPigeonsByCountriesAndYearsAndRingNumbersAsync(IEnumerable<Pigeon> pigeons);
    }
}
