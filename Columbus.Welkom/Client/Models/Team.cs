using Columbus.Models;

namespace Columbus.Welkom.Client.Models
{
    public class Team
    {
        public Team()
        {

        }

        public Team(Owner firstOwner, Owner secondOwner, Owner thirdOwner)
        {
            FirstOwner = firstOwner;
            SecondOwner = secondOwner;
            ThirdOwner = thirdOwner;
        }

        public Owner? FirstOwner { get; set; }
        public Owner? SecondOwner { get; set; }
        public Owner? ThirdOwner { get; set; }
        public int FirstOwnerPoints { get; set; }
        public int SecondOwnerPoints { get; set; }
        public int ThirdOwnerPoints { get; set; }

        public int TotalPoints => FirstOwnerPoints + SecondOwnerPoints + ThirdOwnerPoints;

        public bool OwnerIsInTeam(Owner owner) => FirstOwner?.ID == owner.ID || SecondOwner?.ID == owner.ID || ThirdOwner?.ID == owner.ID;
    }
}
