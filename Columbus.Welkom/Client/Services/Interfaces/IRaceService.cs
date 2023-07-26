using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IRaceService
    {
        Task<IEnumerable<Race>> ReadRacesFromDirectory();
    }
}