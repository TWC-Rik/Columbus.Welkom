using Columbus.Models;

namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IOwnerService : IBaseService<IEnumerable<Owner>>
    {
        Task AddOwnerPigeonsAsync(Owner owner);
        Task<IEnumerable<Pigeon>> GetAllPigeonsAsync();
        Task<IEnumerable<Owner>> ReadOwnersFromFile();
    }
}