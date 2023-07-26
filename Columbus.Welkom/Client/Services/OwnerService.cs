using Columbus.Models;
using Columbus.UDP.Interfaces;
using Columbus.UDP;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using Columbus.Welkom.Client.Services.Interfaces;
using Blazored.LocalStorage;
using Columbus.Welkom.Client.Repositories.Interfaces;

namespace Columbus.Welkom.Client.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IFileSystemAccessService _fileSystemAccessService;
        private readonly IPigeonRepository _pigeonRepository;

        public OwnerService(IFileSystemAccessService fileSystemAccessService, ISyncLocalStorageService localStorageService, IPigeonRepository pigeonRepository)
        {
            _fileSystemAccessService = fileSystemAccessService;
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

        public void AddOwners(IEnumerable<Owner> owners, int year) { }
    }
}
