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

        public PigeonEntity(Pigeon pigeon, int ownerId)
        {
            Year = pigeon.Year;
            Country = pigeon.Country;
            RingNumber = pigeon.RingNumber;
            Chip = pigeon.Chip;
            Sex = pigeon.Sex;
            OwnerId = ownerId;
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

        public bool IsPigeon(Pigeon pigeon) => pigeon.Country == Country && pigeon.Year == Year && pigeon.RingNumber == RingNumber;

        /// <summary>
        /// Overriden to only include country, year, and ringnumber.
        /// This makes comparison between entities easier, especially using hashsets for performant inclusion checks.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();

            hashCode.Add(Country);
            hashCode.Add(Year);
            hashCode.Add(RingNumber);

            return hashCode.ToHashCode();
        }
    }
}
