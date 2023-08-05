using Columbus.Models;

namespace Columbus.Welkom.Client.Models.Entities
{
    public class OwnerEntity
    {
        public OwnerEntity() { }

        public OwnerEntity(Owner owner, int year)
        {
            Id = owner.ID;
            Year = year;
            Name = owner.Name;
            Latitude = owner.Coordinate?.Lattitude ?? 0;
            Longitude = owner.Coordinate?.Longitude ?? 0;
            Club = owner.Club;
            Pigeons = owner.Pigeons.Select(p => new PigeonEntity(p, owner)).ToList();
        }

        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Club { get; set; }

        public ICollection<PigeonEntity>? Pigeons { get; set; }

        public Owner ToOwner() => new Owner(Id, Name, new Coordinate(Longitude, Latitude), Club, Pigeons?.Select(p => p.ToPigeon()).ToList() ?? new List<Pigeon>());
    }
}
