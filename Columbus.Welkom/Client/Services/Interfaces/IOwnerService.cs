using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetOwnersWithAllPigeonsAsync();
        Task<IEnumerable<Owner>> GetOwnersByYearWithYearPigeonsAsync(int year);
        Task<IEnumerable<Owner>> GetOwnersByYearWithYoungPigeonsAsync(int year);
        Task OverwriteOwnersAsync(IEnumerable<Owner> owners);
        Task<IEnumerable<Owner>> ReadOwnersFromFile();
    }
}