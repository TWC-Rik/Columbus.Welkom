using Columbus.Models;

namespace Columbus.Welkom.Client.Models.Entities
{
    public class OwnerEntity : BaseEntity
    {
        public OwnerEntity() { }

        public OwnerEntity(Owner owner)
        {
            Id = owner.ID;
            Name = owner.Name;
            Latitude = owner.Coordinate?.Lattitude ?? 0;
            Longitude = owner.Coordinate?.Longitude ?? 0;
            Club = owner.Club;
        }

        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Club { get; set; }

        public ICollection<PigeonEntity>? Pigeons { get; set; }

        public Owner ToOwner() => new Owner(Id, Name, new Coordinate(Longitude, Latitude), Club, Pigeons?.Select(p => p.ToPigeon()).ToList() ?? new List<Pigeon>());
    }
}
