namespace Columbus.Welkom.Client.Models.Entities
{
    public class LeagueEntity : BaseEntity
    {
        public int Year { get; set; }
        public int OwnerId { get; set; }
        public League League { get; set; }

        public OwnerEntity? Owner { get; set; }
    }
}
