using Columbus.Models;

namespace Columbus.Welkom.Client.Models.Entities
{
    public class RaceEntity
    {
        public RaceEntity() { }

        public RaceEntity(Race race)
        {
            Number = race.Number ?? 0;
            Code = race.Code;
            Name = race.Name;
            Type = race.Type;
            StartTime = race.StartTime;
            Latitude = race.Location.Lattitude;
            Longitude = race.Location.Longitude;
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public RaceType Type { get; set; }
        public DateTime StartTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<PigeonRaceEntity>? PigeonRaces { get; set; }

        public Race ToRace()
        {
            if (PigeonRaces is null)
                throw new ArgumentNullException("PigeonRaces cannot be null");
            if (PigeonRaces.Any(pr => pr.Pigeon is null))
                throw new ArgumentNullException("Pigeon on PigeonProperty cannot be null");
            if (PigeonRaces.Any(pr => pr.Pigeon!.Owner is null))
                throw new ArgumentNullException("Owner on Pigeon on PigeonProperty cannot be null");

            Coordinate startLocation = new Coordinate(Longitude, Latitude);

            IList<OwnerRace> ownerRaces = PigeonRaces.Select(pr => pr.Pigeon!.Owner)
                .Distinct()
                .Select(o => o!.ToOwner())
                .Select(o => new OwnerRace(o, startLocation, PigeonRaces.Count(pr => pr.Pigeon!.OwnerId == o.ID), 0))
                .ToList();

            return new Race(Name, Code, StartTime, startLocation, ownerRaces, PigeonRaces?.Select(pr => pr.ToPigeonRace()).ToList() ?? new List<PigeonRace>());
        }
    }
}
