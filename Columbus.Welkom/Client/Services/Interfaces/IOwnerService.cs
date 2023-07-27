using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetOwnersByYearAsync(int year);
        Task OverwriteOwnersAsync(IEnumerable<Owner> owners, int year);
        Task<IEnumerable<Owner>> ReadOwnersFromFile();
    }
}