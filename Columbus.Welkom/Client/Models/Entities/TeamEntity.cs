namespace Columbus.Welkom.Client.Models.Entities
{
    public class TeamEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int FirstOwnerId { get; set; }
        public int SecondOwnerId { get; set; }
        public int ThirdOwnerId { get; set; }

        public OwnerEntity? FirstOwner { get; set; }
        public OwnerEntity? SecondOwner { get; set; }
        public OwnerEntity? ThirdOwner { get; set; }
    }
}
