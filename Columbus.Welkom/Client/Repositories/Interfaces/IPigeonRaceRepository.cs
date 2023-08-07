using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IPigeonRaceRepository : IBaseRepository<PigeonRaceEntity>
    {
        Task DeleteAllByRaceId(int raceId);
        Task<IEnumerable<PigeonRaceEntity>> GetAllByIdsAsync(IEnumerable<int> ids);
    }
}
