using Columbus.Models;
using Columbus.UDP.Interfaces;
using Columbus.UDP;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using Columbus.Welkom.Client.Services.Interfaces;
using Blazored.LocalStorage;

namespace Columbus.Welkom.Client.Services
{
    public class OwnerService : BaseService<IEnumerable<Owner>>, IOwnerService
    {
        private readonly IFileSystemAccessService _fileSystemAccessService;

        public OwnerService(IFileSystemAccessService fileSystemAccessService, ISyncLocalStorageService localStorageService): base(localStorageService)
        {
            _fileSystemAccessService = fileSystemAccessService;
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

        protected override string GetStorageKey(int club, int year) => $"OWNERS_{club}_{year}";
    }
}
