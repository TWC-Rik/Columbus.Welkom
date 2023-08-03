namespace Columbus.Welkom.Client.Models.Entities
{
    public class PigeonSwapEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int PlayerId { get; set; }
        public int OwnerId { get; set; }
        public int PigeonId { get; set; }
        public int CoupledPlayerId { get; set; }

        public OwnerEntity? Player { get; set; }
        public OwnerEntity? Owner { get; set; }
        public PigeonEntity? Pigeon { get; set; }
        public OwnerEntity? CoupledPlayer { get; set; }
    }
}
