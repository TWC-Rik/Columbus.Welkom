using Blazored.LocalStorage;
using Columbus.Welkom.Client.Services.Interfaces;

namespace Columbus.Welkom.Client.Services
{
    public abstract class BaseService<T> : IBaseService<T>
    {
        private readonly ISyncLocalStorageService _storageService;

        public BaseService(ISyncLocalStorageService localStorageService)
        {
            _storageService = localStorageService;
        }

        public T? GetStorage(int club, int year)
        {
            string yearKey = GetStorageKey(club, year);

            if (_storageService.ContainKey(yearKey))
                return _storageService.GetItem<T>(yearKey);
            else
                return default;
        }

        public void SetStorage(T item, int club, int year)
        {
            _storageService.SetItem(GetStorageKey(club, year), item);
        }

        protected abstract string GetStorageKey(int club, int year);
    }
}
