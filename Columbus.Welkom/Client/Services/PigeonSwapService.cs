using Columbus.Models;
using Columbus.Welkom.Client.Export;
using Columbus.Welkom.Client.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Columbus.Welkom.Client.Services.Interfaces;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;

namespace Columbus.Welkom.Client.Services
{
    public class PigeonSwapService : IPigeonSwapService
    {
        private readonly IFileSystemAccessService _fileSystemAccessService;
        private readonly IPigeonRepository _pigeonRepository;
        private readonly IPigeonSwapRepository _pigeonSwapRepository;
        private readonly IRaceRepository _raceRepository;

        public PigeonSwapService(IFileSystemAccessService fileSystemAccessService, IPigeonRepository pigeonRepository, IPigeonSwapRepository pigeonSwapRepository, IRaceRepository raceRepository)
        {
            _fileSystemAccessService = fileSystemAccessService;
            _pigeonRepository = pigeonRepository;
            _pigeonSwapRepository = pigeonSwapRepository;
            _raceRepository = raceRepository;
        }

        public async Task<IEnumerable<PigeonSwapPair>> GetPigeonSwapPairsByYearAsync(int year)
        {
            IEnumerable<PigeonSwapEntity> pigeonSwapEntities = await _pigeonSwapRepository.GetAllByYearAsync(year);

            IEnumerable<RaceEntity> raceEntities = await _raceRepository.GetAllByYearAndTypes(year, new[] { RaceType.L });
            IEnumerable<Race> races = raceEntities.Select(r => r.ToRace());

            List<PigeonSwapPair> pigeonSwapPairs = pigeonSwapEntities.Select(ps => ps.ToPigeonSwapPair()).ToList();

            Dictionary<Pigeon, PigeonSwapPair> pigeonPigeonSwapPairs = pigeonSwapPairs.ToDictionary(ps => ps.Pigeon!, ps => ps);
            HashSet<Pigeon> pigeonsInPairs = pigeonSwapPairs.Select(ps => ps.Pigeon!).ToHashSet();

            foreach (Race race in races)
            {
                IEnumerable<PigeonRace> pigeonRaces = race.PigeonRaces.Where(pr => pigeonsInPairs.Contains(pr.Pigeon))
                    .OrderByDescending(pr => pr.Speed);
                SimpleRace simpleRace = new SimpleRace(race.Number, race.Type, race.Name, race.Code, race.StartTime, race.Location, race.OwnerRaces.Count, race.PigeonRaces.Count);

                int prizeCount = pigeonRaces.Where(pr => pr.ArrivalTime != DateTime.MinValue).Count();
                double pointStep = 170 / Math.Max(prizeCount - 1, 1);
                int i = 0;
                foreach (PigeonRace pigeonRace in pigeonRaces)
                {
                    int points = Convert.ToInt32(Math.Round(200.0 - pointStep * i++));
                    pigeonPigeonSwapPairs[pigeonRace.Pigeon].RacePoints!.Add(simpleRace, points);
                }
            }

            return pigeonSwapPairs.OrderByDescending(ps => ps.Points);
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

        public async Task ExportToPdf(IEnumerable<PigeonSwapPair> pigeonSwapPairs)
        {
            PigeonSwapDocument document = new PigeonSwapDocument(pigeonSwapPairs);
            byte[] data = document.GetDocument();

            FileSystemOptions options = new FileSystemOptions()
            {

            };

            FileSystemFileHandle fileSystemFileHandle = await _fileSystemAccessService.ShowSaveFilePickerAsync();
            FileSystemWritableFileStream fileSystemWritableFileStream = await fileSystemFileHandle.CreateWritableAsync();

            await fileSystemWritableFileStream.WriteAsync(data);
            await fileSystemWritableFileStream.CloseAsync();
        }
    }
}
