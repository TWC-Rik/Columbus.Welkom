namespace Columbus.Welkom.Client.Models.Entities
{
    public class SelectedYearPigeonEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int OwnerId { get; set; }
        public int PigeonId { get; set; }

        public OwnerEntity? Owner { get; set; }
        public PigeonEntity? Pigeon { get; set; }
    }
}
