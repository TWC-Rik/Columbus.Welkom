using Columbus.Models;
using Columbus.Welkom.Client.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface ISelectedYearPigeonService
    {
        Task DeleteOwnerPigeonPairForYearAsync(int year, Pigeon pigeon);
        Task DeleteOwnerPigeonPairForYearAsync(int year, OwnerPigeonPair ownerPigeonPair);
        Task<IEnumerable<OwnerPigeonPair>> GetOwnerPigeonPairsByYearAsync(int year);
        Task UpdatePigeonForOwnerAsync(int year, OwnerPigeonPair ownerPigeonPair);
    }
}
