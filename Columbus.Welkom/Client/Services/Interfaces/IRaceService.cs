using Columbus.Models;
using Columbus.Welkom.Client.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IRaceService
    {
        Task<IEnumerable<SimpleRace>> GetAllRacesByYearAsync(int year);
        Task<Race> GetRaceByCodeAndYear(string code, int year);
        Task OverwriteRacesAsync(IEnumerable<Race> races, int year);
        Task<Race> ReadRaceFromFileAsync();
        Task<IEnumerable<Race>> ReadRacesFromDirectoryAsync();
        Task StoreRaceAsync(Race race);
    }
}