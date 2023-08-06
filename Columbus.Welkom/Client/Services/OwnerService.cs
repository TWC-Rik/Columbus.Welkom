using Columbus.Models;
using Columbus.UDP.Interfaces;
using Columbus.UDP;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using Columbus.Welkom.Client.Services.Interfaces;
using Blazored.LocalStorage;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Columbus.Welkom.Client.Models.Entities;
using KristofferStrube.Blazor.Streams;
using System.Text;

namespace Columbus.Welkom.Client.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IFileSystemAccessService _fileSystemAccessService;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPigeonRepository _pigeonRepository;

        public OwnerService(IFileSystemAccessService fileSystemAccessService, ISyncLocalStorageService localStorageService, IOwnerRepository ownerRepository, IPigeonRepository pigeonRepository)
        {
            _fileSystemAccessService = fileSystemAccessService;
            _ownerRepository = ownerRepository;
            _pigeonRepository = pigeonRepository;
        }

        public async Task<IEnumerable<Owner>> ReadOwnersFromFile()
        {
            OpenFilePickerOptionsStartInWellKnownDirectory options = new()
            {
                Multiple = false
            };

            FileSystemFileHandle[] fileHandles;
            try
            {
                fileHandles = await _fileSystemAccessService.ShowOpenFilePickerAsync(options);
            }
            catch (Exception)
            {
                throw;
            }

            FileSystemFileHandle fileHandle = fileHandles.Single();

            var file = await fileHandle.GetFileAsync();
            ReadableStream readableStream = await file.StreamAsync();
            var stream = new StreamReader(readableStream, Encoding.Latin1, false, 1_000_000);

            IOwnerReader ownerReader = new OwnerReader();
            return await ownerReader.GetOwnersAsync(stream);
        }

        public async Task<IEnumerable<Owner>> GetOwnersByYearWithAllPigeonsAsync(int year)
        {
            IEnumerable<OwnerEntity> owners = await _ownerRepository.GetAllByYearWithAllPigeonsAsync(year);

            return owners.Select(o => o.ToOwner());
        }

        public async Task<IEnumerable<Owner>> GetOwnersByYearWithYearPigeonsAsync(int year)
        {
            IEnumerable<OwnerEntity> owners = await _ownerRepository.GetAllByYearWithYearPigeonsAsync(year);

            return owners.Select(o => o.ToOwner());
        }

        public async Task<IEnumerable<Owner>> GetOwnersByYearWithYoungPigeonsAsync(int year)
        {
            IEnumerable<OwnerEntity> owners = await _ownerRepository.GetAllByYearWithYoungPigeonsAsync(year);

            return owners.Select(o => o.ToOwner());
        }

        public async Task OverwriteOwnersAsync(IEnumerable<Owner> owners, int year)
        {
            await _ownerRepository.DeleteRangeByYearAsync(year);

            List<OwnerEntity> ownerEntities = owners.Select(o => new OwnerEntity(o, year))
                .ToList();
            List<PigeonEntity> pigeonEntities = owners.SelectMany(o => o.Pigeons.Select(p => new PigeonEntity(p, o.ID)))
                .ToList();

            await _ownerRepository.AddRangeAsync(ownerEntities);
            await _pigeonRepository.AddRangeAsync(pigeonEntities);
        }
    }
}
