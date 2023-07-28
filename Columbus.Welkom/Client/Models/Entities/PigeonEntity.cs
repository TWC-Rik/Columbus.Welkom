using Columbus.Models;

namespace Columbus.Welkom.Client.Models.Entities
{
    public class PigeonEntity
    {
        public PigeonEntity() { }

        public PigeonEntity(Pigeon pigeon, Owner owner)
        {
            Year = pigeon.Year;
            Country = pigeon.Country;
            RingNumber = pigeon.RingNumber;
            Chip = pigeon.Chip;
            Sex = pigeon.Sex;
            OwnerId = owner.ID;
        }

        public int Id { get; set; }
        public int Year { get; set; }
        public string Country { get; set; } = string.Empty;
        public int RingNumber { get; set; }
        public int Chip { get; set; }
        public Sex Sex { get; set; }
        public int OwnerId { get; set; }

        public OwnerEntity? Owner { get; set; }
        public ICollection<PigeonRaceEntity>? PigeonRaces { get; set; }

        public Pigeon ToPigeon() => new Pigeon(Country, Year, RingNumber, Chip, Sex);
    }
}
