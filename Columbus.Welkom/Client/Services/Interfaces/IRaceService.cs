using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IRaceService : IBaseService<IEnumerable<Race>>
    {
        Task<IEnumerable<Race>> ReadRacesFromDirectory();
    }
}