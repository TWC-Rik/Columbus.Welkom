using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetOwnersWithAllPigeonsAsync();
        Task<IEnumerable<Owner>> GetOwnersByYearWithYearPigeonsAsync(int year, bool includeOwnersWithoutPigeons = false);
        Task<IEnumerable<Owner>> GetOwnersByYearWithYoungPigeonsAsync(int year, bool includeOwnersWithoutPigeons = false);
        Task OverwriteOwnersAsync(IEnumerable<Owner> owners);
        Task<IEnumerable<Owner>> ReadOwnersFromFile();
    }
}