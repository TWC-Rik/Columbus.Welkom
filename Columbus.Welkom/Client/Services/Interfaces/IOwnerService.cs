using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> ReadOwnersFromFile();
    }
}