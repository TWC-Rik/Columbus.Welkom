using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IOwnerService : IBaseService<IEnumerable<Owner>>
    {
        Task<IEnumerable<Owner>> ReadOwnersFromFile();
    }
}