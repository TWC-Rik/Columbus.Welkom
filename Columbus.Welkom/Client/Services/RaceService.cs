using Blazored.LocalStorage;
using Columbus.Models;
using Columbus.UDP;
using Columbus.Welkom.Client.Services.Interfaces;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystemAccess;
using System.Text;

namespace Columbus.Welkom.Client.Services
{
    public class RaceService : BaseService<IEnumerable<Race>>, IRaceService
    {
        private readonly IFileSystemAccessService _fileSystemAccessService;
        private const string RACES_KEY = "RACES_";

        public RaceService(IFileSystemAccessService fileSystemAccessService, ISyncLocalStorageService localStorageService) : base(localStorageService)
        {
            _fileSystemAccessService = fileSystemAccessService;
        }

        public async Task<IEnumerable<Race>> ReadRacesFromDirectory()
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

        protected override string GetStorageKey(int club, int year) => $"{RACES_KEY}{year}";
    }
}
