using Columbus.Models;

namespace Columbus.Welkom.Client.Models.Entities
{
    public class PigeonEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Country { get; set; } = string.Empty;
        public int RingNumber { get; set; }
        public string Chip { get; set; } = string.Empty;
        public Sex Sex { get; set; }
        public int OwnerId { get; set; }

        public OwnerEntity? Owner { get; set; }
    }
}
