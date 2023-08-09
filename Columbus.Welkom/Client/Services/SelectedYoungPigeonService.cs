using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Models;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Columbus.Welkom.Client.Services.Interfaces;
using Columbus.Models;

namespace Columbus.Welkom.Client.Services
{
    public class SelectedYoungPigeonService : ISelectedYoungPigeonService
    {
        private readonly IPigeonRepository _pigeonRepository;
        private readonly IRaceRepository _raceRepository;
        private readonly ISelectedYoungPigeonRepository _selectedYoungPigeonRepository;

        public SelectedYoungPigeonService(IPigeonRepository pigeonRepository, IRaceRepository raceRepository, ISelectedYoungPigeonRepository selectedYoungPigeonRepository)
        {
            _pigeonRepository = pigeonRepository;
            _raceRepository = raceRepository;
            _selectedYoungPigeonRepository = selectedYoungPigeonRepository;
        }

        public async Task<IEnumerable<OwnerPigeonPair>> GetOwnerPigeonPairsByYearAsync(int year)
        {
            IEnumerable<SelectedYoungPigeonEntity> selectedYoungPigeonEntities = await _selectedYoungPigeonRepository.GetAllByYearAsync(year);
            IEnumerable<RaceEntity> raceEntities = await _raceRepository.GetAllByYearAndTypes(year, new[] {
                RaceType.J,
                RaceType.L,
                RaceType.F
            });
            IEnumerable<Race> races = raceEntities.Select(re => re.ToRace());

            List<OwnerPigeonPair> ownerPigeonPairs = selectedYoungPigeonEntities.Select(syp => new OwnerPigeonPair(syp.Owner!.ToOwner(), syp.Pigeon!.ToPigeon()))
                .ToList();

            foreach (Race race in races)
            {
                Dictionary<Pigeon, PigeonRace> pigeonRaces = race.PigeonRaces.ToDictionary(pr => pr.Pigeon, pr => pr);

                foreach (OwnerPigeonPair pair in ownerPigeonPairs)
                {
                    if (pigeonRaces.ContainsKey(pair.Pigeon!))
                        pair.Points += pigeonRaces[pair.Pigeon!].Points ?? 0;
                }
            }

            return ownerPigeonPairs.OrderByDescending(pair => pair.Points);
        }

        public async Task UpdatePigeonForOwnerAsync(int year, OwnerPigeonPair ownerPigeonPair)
        {
            if (ownerPigeonPair.Owner is null)
                return;

            SelectedYoungPigeonEntity? selectedYearPigeonEntity = await _selectedYoungPigeonRepository.GetByYearAndOwnerAsync(year, ownerPigeonPair.Owner.ID);

            PigeonEntity pigeon = await _pigeonRepository.GetByCountryAndYearAndRingNumberAsync(ownerPigeonPair.Pigeon!.Country, ownerPigeonPair.Pigeon.Year, ownerPigeonPair.Pigeon.RingNumber);

            if (selectedYearPigeonEntity is null)
            {
                await _selectedYoungPigeonRepository.AddAsync(new SelectedYoungPigeonEntity()
                {
                    Year = year,
                    OwnerId = ownerPigeonPair.Owner.ID,
                    PigeonId = pigeon.Id
                });
            }
            else
            {
                selectedYearPigeonEntity.PigeonId = pigeon.Id;
                await _selectedYoungPigeonRepository.UpdateAsync(selectedYearPigeonEntity);
            }
        }

        public async Task DeleteOwnerPigeonPairForYearAsync(int year, Pigeon pigeon)
        {
            SelectedYoungPigeonEntity? oldPair = await _selectedYoungPigeonRepository.GetByYearAndPigeonAsync(year, pigeon.Year, pigeon.Country, pigeon.RingNumber);

            if (oldPair is null)
                throw new ArgumentNullException("No pair with this pigeon present.");

            await _selectedYoungPigeonRepository.DeleteByYearAndOwnerAsync(year, oldPair.Owner!.Id);
        }

        public async Task DeleteOwnerPigeonPairForYearAsync(int year, OwnerPigeonPair ownerPigeonPair)
        {
            if (ownerPigeonPair.Owner is null)
                throw new ArgumentNullException("Owner cannot be null");

            await _selectedYoungPigeonRepository.DeleteByYearAndOwnerAsync(year, ownerPigeonPair.Owner.ID);
        }
    }
}
