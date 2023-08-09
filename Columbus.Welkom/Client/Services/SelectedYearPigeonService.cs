using Columbus.Models;
using Columbus.Welkom.Client.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Columbus.Welkom.Client.Services.Interfaces;

namespace Columbus.Welkom.Client.Services
{
    public class SelectedYearPigeonService : ISelectedYearPigeonService
    {
        private readonly IPigeonRepository _pigeonRepository;
        private readonly IRaceRepository _raceRepository;
        private readonly ISelectedYearPigeonRepository _selectedYearPigeonRepository;

        public SelectedYearPigeonService(IPigeonRepository pigeonRepository, IRaceRepository raceRepository, ISelectedYearPigeonRepository selectedYearPigeonRepository)
        {
            _pigeonRepository = pigeonRepository;
            _raceRepository = raceRepository;
            _selectedYearPigeonRepository = selectedYearPigeonRepository;
        }

        public async Task<IEnumerable<OwnerPigeonPair>> GetOwnerPigeonPairsByYearAsync(int year)
        {
            IEnumerable<SelectedYearPigeonEntity> selectedYearPigeonEntities = await _selectedYearPigeonRepository.GetAllByYearAsync(year);
            IEnumerable<RaceEntity> raceEntities = await _raceRepository.GetAllByYearAndTypes(year, new[] { 
                RaceType.V,
                RaceType.M,
                RaceType.E,
                RaceType.O,
                RaceType.X,
                RaceType.N
            });
            IEnumerable<Race> races = raceEntities.Select(re => re.ToRace());

            List<OwnerPigeonPair> ownerPigeonPairs = selectedYearPigeonEntities.Select(syp => new OwnerPigeonPair(syp.Owner!.ToOwner(), syp.Pigeon!.ToPigeon()))
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

            SelectedYearPigeonEntity? selectedYearPigeonEntity = await _selectedYearPigeonRepository.GetByYearAndOwnerAsync(year, ownerPigeonPair.Owner.ID);

            PigeonEntity pigeon = await _pigeonRepository.GetByCountryAndYearAndRingNumberAsync(ownerPigeonPair.Pigeon!.Country, ownerPigeonPair.Pigeon.Year, ownerPigeonPair.Pigeon.RingNumber);

            if (selectedYearPigeonEntity is null)
            {
                await _selectedYearPigeonRepository.AddAsync(new SelectedYearPigeonEntity()
                {
                    Year = year,
                    OwnerId = ownerPigeonPair.Owner.ID,
                    PigeonId = pigeon.Id
                });
            } else
            {
                selectedYearPigeonEntity.PigeonId = pigeon.Id;
                await _selectedYearPigeonRepository.UpdateAsync(selectedYearPigeonEntity);
            }
        }

        public async Task DeleteOwnerPigeonPairForYearAsync(int year, Pigeon pigeon)
        {
            SelectedYearPigeonEntity? oldPair = await _selectedYearPigeonRepository.GetByYearAndPigeonAsync(year, pigeon.Year, pigeon.Country, pigeon.RingNumber);

            if (oldPair is null)
                throw new ArgumentNullException("No pair with this pigeon present.");

            await _selectedYearPigeonRepository.DeleteByYearAndOwnerAsync(year, oldPair.Owner!.Id);
        }

        public async Task DeleteOwnerPigeonPairForYearAsync(int year, OwnerPigeonPair ownerPigeonPair)
        {
            if (ownerPigeonPair.Owner is null)
                throw new ArgumentNullException("Owner cannot be null");

            await _selectedYearPigeonRepository.DeleteByYearAndOwnerAsync(year, ownerPigeonPair.Owner.ID);
        }
    }
}
