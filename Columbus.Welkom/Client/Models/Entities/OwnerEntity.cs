namespace Columbus.Welkom.Client.Models.Entities
{
    public class OwnerEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Club { get; set; }
    }
}
