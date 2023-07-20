using Columbus.Models;

namespace Columbus.Welkom.Models
{
    public class Leagues
    {
        public IDictionary<Owner, int> FirstLeagueOwners { get; set; }
        public IDictionary<Owner, int> SecondLeagueOwners { get; set; }
        public IDictionary<Owner, int> ThirdLeagueOwners { get; set; }
    }
}