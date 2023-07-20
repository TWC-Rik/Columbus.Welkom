using Columbus.Models;
using System.Text.Json.Serialization;

namespace Columbus.Welkom.Client.Models
{
    public class LeagueOwner
    {
        [JsonConstructor]
        public LeagueOwner(int ownerID, League league, int points)
        {
            OwnerID = ownerID;
            League = league;
            Points = points;
        }

        public LeagueOwner(Owner owner, League league, int points)
        {
            OwnerID = owner.ID;
            League = league;
            Points = points;
        }

        public int OwnerID { get; set; }
        public League League { get; set; }
        public int Points { get; set; }
    }
}
