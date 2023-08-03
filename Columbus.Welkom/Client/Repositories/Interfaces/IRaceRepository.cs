using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IRaceRepository : IBaseRepository<RaceEntity>
    {
        Task<int> DeleteRangeByYearAsync(int year);
        Task<IEnumerable<SimpleRaceEntity>> GetAllByYearAsync(int year);
        Task<RaceEntity> GetByCodeAndYear(string code, int year);
    }
}