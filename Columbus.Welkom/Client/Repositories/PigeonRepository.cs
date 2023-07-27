using Columbus.Models;
using Columbus.Welkom.Client.DataContext;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.Repositories
{
    public class PigeonRepository : IPigeonRepository
    {
        private readonly PigeonContext _dbContext;

        public PigeonRepository(PigeonContext context)
        {
            _dbContext = context;
        }

        public async Task AddRangeAsync(IEnumerable<PigeonEntity> pigeons)
        {
            await _dbContext.AddRangeAsync(pigeons);
        }
    }
}
