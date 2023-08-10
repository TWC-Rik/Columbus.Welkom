using Columbus.Welkom.Client.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Columbus.Welkom.Client.Services.Interfaces;

namespace Columbus.Welkom.Client.Services
{
    public class PigeonSwapService : IPigeonSwapService
    {
        private readonly IPigeonRepository _pigeonRepository;
        private readonly IPigeonSwapRepository _pigeonSwapRepository;

        public PigeonSwapService(IPigeonRepository pigeonRepository, IPigeonSwapRepository pigeonSwapRepository)
        {
            _pigeonRepository = pigeonRepository;
            _pigeonSwapRepository = pigeonSwapRepository;
        }

        public async Task<IEnumerable<PigeonSwapPair>> GetPigeonSwapPairsByYearAsync(int year)
        {
            IEnumerable<PigeonSwapEntity> pigeonSwapEntities = await _pigeonSwapRepository.GetAllByYearAsync(year);

            return pigeonSwapEntities.Select(ps => ps.ToPigeonSwapPair());
        }

        public async Task UpdatePigeonSwapPairAsync(int year, PigeonSwapPair pigeonSwapPair)
        {
            if (pigeonSwapPair.Pigeon is null || pigeonSwapPair.CoupledPlayer is null)
                return;

            if (pigeonSwapPair.Player is null || pigeonSwapPair.Owner is null)
                throw new ArgumentNullException("PigeonSwapPair Player, Owner, Pigeon, and CoupledOwner cannot be null.");

            PigeonEntity pigeon = await _pigeonRepository.GetByCountryAndYearAndRingNumberAsync(pigeonSwapPair.Pigeon!.Country, pigeonSwapPair.Pigeon.Year, pigeonSwapPair.Pigeon.RingNumber);

            if (pigeonSwapPair.Id is null)
            {
                await _pigeonSwapRepository.AddAsync(new PigeonSwapEntity()
                {
                    Year = year,
                    PlayerId = pigeonSwapPair.Player.ID,
                    OwnerId = pigeonSwapPair.Owner.ID,
                    PigeonId = pigeon.Id,
                    CoupledPlayerId = pigeonSwapPair.CoupledPlayer.ID
                });
            }
            else
            {
                PigeonSwapEntity? pigeonSwapEntity = await _pigeonSwapRepository.GetByIdAsync(pigeonSwapPair.Id.Value);

                if (pigeonSwapEntity is null)
                    throw new ArgumentException("No PigeonSwap entry by this id.");

                pigeonSwapEntity.PigeonId = pigeon.Id;
                pigeonSwapEntity.CoupledPlayerId = pigeonSwapPair.CoupledPlayer.ID;
                await _pigeonSwapRepository.UpdateAsync(pigeonSwapEntity);
            }
        }

        public async Task DeletePigeonSwapPairForYearAsync(int year, PigeonSwapPair pigeonSwapPair)
        {
            if (pigeonSwapPair.Id is null)
                return;

            if (pigeonSwapPair.Player is null || pigeonSwapPair.Pigeon is null)
                throw new ArgumentNullException("PigeonSwapPair player and pigeon cannot be null");

            await _pigeonSwapRepository.DeleteByYearAndPlayerAndPigeonAsync(year, pigeonSwapPair.Player.ID, pigeonSwapPair.Pigeon.Country, pigeonSwapPair.Pigeon.Year, pigeonSwapPair.Pigeon.RingNumber);
        }
    }
}
