using Blazored.LocalStorage;
using Columbus.Welkom.Client.Models;
using Columbus.Welkom.Client.Services.Interfaces;

namespace Columbus.Welkom.Client.Services
{
    public class LeaguesService : BaseService<Leagues>, ILeaguesService
    {
        public LeaguesService(ISyncLocalStorageService localStorageService) : base(localStorageService)
        {

        }

        protected override string GetStorageKey(int club, int year) => $"LEAGUES_{club}_{year}";
    }
}
