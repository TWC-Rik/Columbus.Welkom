using Columbus.Models;
using Columbus.UDP;
using Columbus.UDP.Interfaces;
using Columbus.Welkom.Client.Models;
using Columbus.Welkom.Client.Models.Entities;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Columbus.Welkom.Client.Services.Interfaces;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using KristofferStrube.Blazor.Streams;
using Microsoft.JSInterop;
using System.Diagnostics;
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
            OpenFilePickerOptionsStartInWellKnownDirectory options = new()
            {
                Multiple = false
            };

            FileSystemFileHandle[] fileHandles;
            try
            {
                fileHandles = await _fileSystemAccessService.ShowOpenFilePickerAsync(options);
            } catch (JSException)
            {
                throw;
            }

            FileSystemFileHandle fileHandle = fileHandles.Single();

            return await ReadRaceFromFile(fileHandle);
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
            ReadableStream readableStream = await file.StreamAsync();
            var stream = new StreamReader(readableStream, Encoding.Latin1, false, 1_000_000);

            IRaceReader raceReader = new RaceReader();
            return await raceReader.GetRaceAsync(stream);
        }

        public async Task<IEnumerable<SimpleRace>> GetAllRacesByYearAsync(int year)
        {
            IEnumerable<SimpleRaceEntity> races = await _raceRepository.GetAllByYearAsync(year);

            return races.Select(r => r.ToSimpleRace())
                .OrderBy(r => r.Number);
        }

        public async Task OverwriteRacesAsync(IEnumerable<Race> races, int year)
        {
            await _raceRepository.DeleteRangeByYearAsync(year);

            IEnumerable<Pigeon> pigeonData = races.SelectMany(r => r.PigeonRaces)
                .Select(pr => pr.Pigeon);
            IEnumerable<PigeonEntity> pigeonsInRaces = await _pigeonRepository.GetPigeonsByCountriesAndYearsAndRingNumbersAsync(pigeonData);

            await _raceRepository.AddRangeAsync(races.Select(r => new RaceEntity(r)));
        }

        public async Task StoreRaceAsync(Race race)
        {
            if (await _raceRepository.IsRaceCodePresentForYear(race.Code, race.StartTime.Year))
                return;

            IEnumerable<Owner> raceOwners = race.OwnerRaces.Select(or => or.Owner);
            await AddMissingOwners(raceOwners, race.StartTime.Year);

            IEnumerable<PigeonEntity> allPigeonsInRace = await AddMissingPigeons(raceOwners);

            RaceEntity addedRace = await _raceRepository.AddAsync(new RaceEntity(race));

            IEnumerable<PigeonRaceEntity> pigeonRacesToAdd = GetPigeonRaceEntities(race.PigeonRaces, allPigeonsInRace, addedRace.Id);

            await _pigeonRaceRepository.AddRangeAsync(pigeonRacesToAdd);
        }

        private async Task<IEnumerable<OwnerEntity>> AddMissingOwners(IEnumerable<Owner> owners, int year)
        {
            IEnumerable<OwnerEntity> existingOwners = await _ownerRepository.GetAllByIdsAsync(owners.Select(o => o.ID));
            HashSet<int> ownerIds = existingOwners.Select(o => o.Id).ToHashSet();

            IEnumerable<Owner> ownersToAdd = owners.Where(o => !ownerIds.Contains(o.ID));

            IEnumerable<OwnerEntity> addedOwners = await _ownerRepository.AddRangeAsync(ownersToAdd.Select(o => new OwnerEntity(o, year)));

            return await _ownerRepository.GetAllByIdsAsync(owners.Select(o => o.ID));
        }

        private async Task<IEnumerable<PigeonEntity>> AddMissingPigeons(IEnumerable<Owner> owners)
        {
            IEnumerable<Pigeon> pigeons = owners.SelectMany(o => o.Pigeons);
            IEnumerable<PigeonEntity> existingPigeons = await _pigeonRepository.GetPigeonsByCountriesAndYearsAndRingNumbersAsync(pigeons);
            HashSet<int> existingPigeonsHashSet = existingPigeons.Select(p => p.GetHashCode()).ToHashSet();

            List<PigeonEntity> pigeonsToAdd = new List<PigeonEntity>();
            foreach (Owner owner in owners)
            {
                foreach (Pigeon pigeon in owner.Pigeons)
                {
                    if (existingPigeonsHashSet.Contains(pigeon.GetHashCode()))
                        continue;

                    pigeonsToAdd.Add(new PigeonEntity(pigeon, owner));
                }
            }

            await _pigeonRepository.AddRangeAsync(pigeonsToAdd);

            return await _pigeonRepository.GetPigeonsByCountriesAndYearsAndRingNumbersAsync(pigeons);
        }

        private IEnumerable<PigeonRaceEntity> GetPigeonRaceEntities(IList<PigeonRace> pigeonRaces, IEnumerable<PigeonEntity> pigeonEntities, int raceId)
        {
            Dictionary<int, PigeonEntity> pigeonEntitiesSet = pigeonEntities.ToDictionary(p => p.GetHashCode(), p => p);

            List<PigeonRaceEntity> pigeonRaceEntities = new List<PigeonRaceEntity>();
            foreach (PigeonRace pr in pigeonRaces)
            {
                bool found = pigeonEntitiesSet.TryGetValue(pr.Pigeon.GetHashCode(), out PigeonEntity? pigeonEntity);
                if (!found)
                    throw new ArgumentException($"Given pigeon list did not contain pigeon {pr.Pigeon}");
                pigeonRaceEntities.Add(new PigeonRaceEntity(pr, pigeonEntity!.Id, raceId));
            }

            return pigeonRaceEntities;
        }

        public async Task<Race> GetRaceByCodeAndYear(string code, int year)
        {
            RaceEntity race = await _raceRepository.GetByCodeAndYear(code, year);

            return race.ToRace();
        }

        public async Task DeleteRaceByCodeAndYear(string code, int year)
        {
            RaceEntity race = await _raceRepository.GetByCodeAndYear(code, year);

            await _pigeonRaceRepository.DeleteAllByRaceId(race.Id);

            await _raceRepository.DeleteRaceByCodeAndYear(code, year);
        }
    }
}
