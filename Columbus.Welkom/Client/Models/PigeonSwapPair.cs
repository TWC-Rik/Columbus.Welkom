using Columbus.Models;

namespace Columbus.Welkom.Client.Models
{
    public class PigeonSwapPair
    {
        public PigeonSwapPair()
        {

        }

        public PigeonSwapPair(Owner owner, Pigeon pigeon, Owner coupledOwner)
        {
            Owner = owner;
            Pigeon = pigeon;
            CoupledOwner = coupledOwner;
        }

        public PigeonSwapPair(Owner owner, Pigeon pigeon, Owner coupledOwner, int points)
        {
            Owner = owner;
            Pigeon = pigeon;
            CoupledOwner = coupledOwner;
            Points = points;
        }
        
        public Owner? Owner { get; set; }
        public Pigeon? Pigeon { get; set; }
        public Owner? CoupledOwner { get; set; }
        public int Points { get; set; }
    }
}
