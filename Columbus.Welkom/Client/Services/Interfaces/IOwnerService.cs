using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetOwnersByYearWithAllPigeonsAsync(int year);
        Task<IEnumerable<Owner>> GetOwnersByYearWithYearPigeonsAsync(int year);
        Task<IEnumerable<Owner>> GetOwnersByYearWithYoungPigeonsAsync(int year);
        Task OverwriteOwnersAsync(IEnumerable<Owner> owners, int year);
        Task<IEnumerable<Owner>> ReadOwnersFromFile();
    }
}