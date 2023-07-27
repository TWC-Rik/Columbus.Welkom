using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IRaceService
    {
        Task<IEnumerable<Race>> GetAllRacesByYear(int year);
        Task OverwriteRaces(IEnumerable<Race> races, int year);
        Task<IEnumerable<Race>> ReadRacesFromDirectory();
    }
}