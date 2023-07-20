using Columbus.Models;
using System.Text.Json.Serialization;

namespace Columbus.Welkom.Client.Models
{
    public class Leagues
    {
        private IEnumerable<LeagueOwner> _participants;

        [JsonConstructor]
        public Leagues(IEnumerable<LeagueOwner> participants)
        {
            _participants = participants;
        }

        [JsonIgnore]
        public IEnumerable<LeagueOwner> FirstLeagueOwners => _participants.Where(p => p.League == League.First).OrderByDescending(p => p.Points);

        [JsonIgnore]
        public IEnumerable<LeagueOwner> SecondLeagueOwners => _participants.Where(p => p.League == League.Second).OrderByDescending(p => p.Points);

        [JsonIgnore]
        public IEnumerable<LeagueOwner> ThirdLeagueOwners => _participants.Where(p => p.League == League.Third).OrderByDescending(p => p.Points);

        public void Promote(Owner owner)
        {
            LeagueOwner participant = GetLeagueOwner(owner);
            participant.League--;
        }

        public void Demote(Owner owner)
        {
            LeagueOwner participant = GetLeagueOwner(owner);
            participant.League--;
        }

        private LeagueOwner GetLeagueOwner(Owner owner) => _participants.First(p => p.OwnerID == owner.ID);

        public void AddOwner(Owner owner)
        {

        }
    }
}