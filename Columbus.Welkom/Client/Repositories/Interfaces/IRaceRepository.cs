using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IRaceRepository : IBaseRepository<RaceEntity>
    {
        Task AddRaceAsync(RaceEntity race);
        Task<int> DeleteRangeByYearAsync(int year);
        Task<IEnumerable<RaceEntity>> GetAllByYearAsync(int year);
    }
}