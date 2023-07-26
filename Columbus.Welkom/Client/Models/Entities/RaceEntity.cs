using Columbus.Models;

namespace Columbus.Welkom.Client.Models.Entities
{
    public class RaceEntity
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Code { get; set; } = string.Empty;
        public RaceType Type { get; set; }
        public DateTime StartTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
