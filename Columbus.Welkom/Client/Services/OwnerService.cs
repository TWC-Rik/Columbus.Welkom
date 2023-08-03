using Columbus.Models;
using Columbus.UDP.Interfaces;
using Columbus.UDP;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using Columbus.Welkom.Client.Services.Interfaces;
using Blazored.LocalStorage;
using Columbus.Welkom.Client.Repositories.Interfaces;
using Columbus.Welkom.Client.Models.Entities;

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
            try
            {
                OpenFilePickerOptionsStartInWellKnownDirectory options = new()
                {
                    Multiple = false,
                    StartIn = WellKnownDirectory.Downloads
                };
                var fileHandles = await _fileSystemAccessService.ShowOpenFilePickerAsync(options);
                FileSystemFileHandle fileHandle = fileHandles.Single();

                var file = await fileHandle.GetFileAsync();
                var fileContent = await file.TextAsync();
                IOwnerReader ownerReader = new OwnerReader(fileContent.Split("\r\n"));
                return ownerReader.GetOwners();
            }
            catch (Exception)
            {
                throw;
            }
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

            await _ownerRepository.AddRangeAsync(owners.Select(o => new OwnerEntity(o, year)));
        }
    }
}
