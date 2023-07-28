using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IPigeonRepository : IBaseRepository<PigeonEntity>
    {
        Task<IEnumerable<PigeonEntity>> GetPigeonsByCountriesAndYearsAndRingNumbers(IEnumerable<Pigeon> pigeons);
    }
}
