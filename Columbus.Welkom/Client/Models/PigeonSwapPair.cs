using Columbus.Models;

namespace Columbus.Welkom.Client.Models
{
    public class PigeonSwapPair
    {
        public PigeonSwapPair()
        {

        }

        public PigeonSwapPair(Owner player, Owner owner, Pigeon pigeon, Owner coupledOwner)
        {
            Player = player;
            Owner = owner;
            Pigeon = pigeon;
            CoupledPlayer = coupledOwner;
        }

        public PigeonSwapPair(int id, Owner player, Owner owner, Pigeon pigeon, Owner coupledOwner)
        {
            Id = id;
            Player = player;
            Owner = owner;
            Pigeon = pigeon;
            CoupledPlayer = coupledOwner;
        }

        public PigeonSwapPair(Owner player, Owner owner, Pigeon pigeon, Owner coupledOwner, int points)
        {
            Player = player;
            Owner = owner;
            Pigeon = pigeon;
            CoupledPlayer = coupledOwner;
            Points = points;
        }

        public PigeonSwapPair(int id, Owner player, Owner owner, Pigeon pigeon, Owner coupledOwner, int points)
        {
            Id = id;
            Player = player;
            Owner = owner;
            Pigeon = pigeon;
            CoupledPlayer = coupledOwner;
            Points = points;
        }
        
        public int? Id { get; set; }
        public Owner? Player { get; set; }
        public Owner? Owner { get; set; }
        public Pigeon? Pigeon { get; set; }
        public Owner? CoupledPlayer { get; set; }
        public int Points { get; set; }
    }
}
