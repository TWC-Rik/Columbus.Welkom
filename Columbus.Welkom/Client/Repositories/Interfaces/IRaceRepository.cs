using Columbus.Models;
using Columbus.Welkom.Client.Models.Entities;

namespace Columbus.Welkom.Client.Repositories.Interfaces
{
    public interface IRaceRepository : IBaseRepository<RaceEntity>
    {
        Task<int> DeleteRaceByCodeAndYear(string code, int year);
        Task<int> DeleteRangeByYearAsync(int year);
        Task<IEnumerable<RaceEntity>> GetAllByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<RaceEntity>> GetAllByYearAndTypes(int year, RaceType[] types);
        Task<IEnumerable<SimpleRaceEntity>> GetAllSimpleByYearAsync(int year);
        Task<IEnumerable<SimpleRaceEntity>> GetAllSimpleByYearAndTypes(int year, RaceType[] types);
        Task<RaceEntity> GetByCodeAndYear(string code, int year);
        Task<bool> IsRaceCodePresentForYear(string code, int year);
    }
}