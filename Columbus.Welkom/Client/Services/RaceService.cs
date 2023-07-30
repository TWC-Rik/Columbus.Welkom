using Columbus.Models;
using Columbus.UDP;
using Columbus.UDP.Interfaces;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Columbus.Welkom.Client.Services.Interfaces;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using System.Linq;
using System.Text;

namespace Columbus.Welkom.Client.Services
{
    public class RaceService : IRaceService
    {
        private readonly IFileSystemAccessService _fileSystemAccessService;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPigeonRepository _pigeonRepository;
        private readonly IPigeonRaceRepository _pigeonRaceRepository;
        private readonly IRaceRepository _raceRepository;

        public RaceService(IFileSystemAccessService fileSystemAccessService, IOwnerRepository ownerRepository, IPigeonRepository pigeonRepository, IPigeonRaceRepository pigeonRaceRepository, IRaceRepository raceRepository)
        {
            _fileSystemAccessService = fileSystemAccessService;
            _ownerRepository = ownerRepository;
            _pigeonRepository = pigeonRepository;
            _pigeonRaceRepository = pigeonRaceRepository;
            _raceRepository = raceRepository;
        }

        public async Task<Race> ReadRaceFromFileAsync()
        {
            try
            {
                OpenFilePickerOptionsStartInWellKnownDirectory options = new()
                {
                    Multiple = false
                };
                var fileHandles = await _fileSystemAccessService.ShowOpenFilePickerAsync(options);
                FileSystemFileHandle fileHandle = fileHandles.Single();

                var file = await fileHandle.GetFileAsync();
                var fileContent = await file.TextAsync();
                IRaceReader raceReader = new RaceReader(fileContent.Split("\r\n"));
                return raceReader.GetRace();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Race>> ReadRacesFromDirectoryAsync()
        {
            try
            {
                FileSystemDirectoryHandle directoryHandle = await _fileSystemAccessService.ShowDirectoryPickerAsync();
                
                IFileSystemHandle[] entries = await directoryHandle.ValuesAsync();

                IEnumerable<IFileSystemHandle> filtered = await FilterByFileExtension(entries);

                List<Race> races = new List<Race>();

                foreach (IFileSystemHandle entry in filtered)
                {
                    FileSystemFileHandle file = await directoryHandle.GetFileHandleAsync(await entry.GetNameAsync());
                    races.Add(await ReadRaceFromFile(file));
                }

                return races;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<IEnumerable<IFileSystemHandle>> FilterByFileExtension(IFileSystemHandle[] fileHandles)
        {
            List<IFileSystemHandle> filtered = new List<IFileSystemHandle>();

            foreach (var fileHandle in fileHandles)
            {
                FileSystemHandleKind kind = await fileHandle.GetKindAsync();
                if (kind != FileSystemHandleKind.File)
                    continue;

                string name = await fileHandle.GetNameAsync();
                if (!name.EndsWith(".udp"))
                    continue;

                filtered.Add(fileHandle);
            }

            return filtered;
        }

        private async Task<Race> ReadRaceFromFile(FileSystemFileHandle fileHandle)
        {
            var file = await fileHandle.GetFileAsync();
            byte[] isoBytes = await file.ArrayBufferAsync();
            string[] udpContent = Encoding.Latin1.GetString(isoBytes)
                .Split("\r\n");

            RaceReader raceReader = new RaceReader(udpContent);
            return raceReader.GetRace();
        }

        public async Task<IEnumerable<Race>> GetAllRacesByYearAsync(int year)
        {
            IEnumerable<RaceEntity> races = await _raceRepository.GetAllByYearAsync(year);

            return races.Select(r => r.ToRace());
        }

        public async Task OverwriteRacesAsync(IEnumerable<Race> races, int year)
        {
            await _raceRepository.DeleteRangeByYearAsync(year);

            IEnumerable<Pigeon> pigeonData = races.SelectMany(r => r.PigeonRaces)
                .Select(pr => pr.Pigeon);
            IEnumerable<PigeonEntity> pigeonsInRaces = await _pigeonRepository.GetPigeonsByCountriesAndYearsAndRingNumbers(pigeonData);

            await _raceRepository.AddRangeAsync(races.Select(r => new RaceEntity(r)));
        }

        public async Task StoreRaceAsync(Race race)
        {
            DateTime A = DateTime.Now;
            // Add the owners in the race which are not in database.
            IEnumerable<Owner> ownerData = race.OwnerRaces.Select(or => or.Owner);
            IEnumerable<OwnerEntity> existingOwners = await _ownerRepository.GetAllByIdsAsync(ownerData.Select(o => o.ID));
            IEnumerable<Owner> ownersToAdd = ownerData.Where(o => !existingOwners.Any(or => or.Id == o.ID));
            await _ownerRepository.AddRangeAsync(ownersToAdd.Select(o => new OwnerEntity(o, race.StartTime.Year)));
            DateTime B = DateTime.Now;
            Console.WriteLine(B - A);

            // Add the pigeons for which are in the race but not in database.
            IEnumerable<Pigeon> pigeonData = race.PigeonRaces.Select(pr => pr.Pigeon);
            IEnumerable<PigeonEntity> pigeonsAlreadyPresent = await _pigeonRepository.GetPigeonsByCountriesAndYearsAndRingNumbers(pigeonData);
            IEnumerable<PigeonEntity> pigeonsToAdd = pigeonData.Where(p => !pigeonsAlreadyPresent.Any(pap => pap.IsPigeon(p)))
                .Select(p => new PigeonEntity(p, ownerData.First(o => o.Pigeons.Contains(p))));
            IEnumerable<PigeonEntity> addedPigeons = await _pigeonRepository.AddRangeAsync(pigeonsToAdd);
            DateTime C = DateTime.Now;
            Console.WriteLine(C - B);

            RaceEntity addedRace = await _raceRepository.AddAsync(new RaceEntity(race));
            DateTime D = DateTime.Now;
            Console.WriteLine(D - C);

            IEnumerable<PigeonEntity> allPigeonsInRace = pigeonsAlreadyPresent.Concat(addedPigeons);
            IEnumerable<PigeonRaceEntity> pigeonRacesToAdd = race.PigeonRaces.Select(pr => new PigeonRaceEntity(pr, allPigeonsInRace.First(p => p.IsPigeon(pr.Pigeon)).Id, addedRace.Id));
            await _pigeonRaceRepository.AddRangeAsync(pigeonRacesToAdd);
            DateTime E = DateTime.Now;
            Console.WriteLine(E - D);
        }

        private async Task<OwnerEntity> AddMissingOwners(IEnumerable<Owner> owners)
        {
            throw new NotImplementedException();
        }

        private async Task<PigeonEntity> AddMissingPigeons(IEnumerable<Pigeon> pigeons)
        {
            throw new NotImplementedException();
        }
    }
}
