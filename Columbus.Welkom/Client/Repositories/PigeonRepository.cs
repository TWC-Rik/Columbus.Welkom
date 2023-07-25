using Columbus.Models;
using Columbus.Welkom.Client.DataContext;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.Repositories
{
    public class PigeonRepository : IBaseRepository<Pigeon>, IPigeonRepository
    {
        private readonly PigeonContext _dbContext;

        public PigeonRepository(PigeonContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Pigeon>> GetAllAsync()
        {
            return await _dbContext.Pigeons.ToListAsync();
        }

        public async Task AddAsync(Pigeon pigeon)
        {
            _dbContext.Pigeons.Add(pigeon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Pigeon> pigeons)
        {
            _dbContext.Pigeons.AddRange(pigeons);
            await _dbContext.SaveChangesAsync();
        }
    }
}
