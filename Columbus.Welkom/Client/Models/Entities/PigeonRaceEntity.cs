namespace Columbus.Welkom.Client.Models.Entities
{
    public class PigeonRaceEntity
    {
        public int Id { get; set; }
        public int PigeonId { get; set; }
        public int RaceId { get; set; }
        public int Mark { get; set; }
        public DateTime ArrivalTime { get; set; }

        public PigeonEntity? Pigeon { get; set; }
    }
}
