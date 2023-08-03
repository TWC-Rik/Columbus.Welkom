using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IRaceService
    {
        Task<IEnumerable<Race>> GetAllRacesByYearAsync(int year);
        Task OverwriteRacesAsync(IEnumerable<Race> races, int year);
        Task<Race> ReadRaceFromFileAsync();
        Task<IEnumerable<Race>> ReadRacesFromDirectoryAsync();
        Task StoreRaceAsync(Race race);
    }
}