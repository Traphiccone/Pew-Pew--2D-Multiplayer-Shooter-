using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unbreakable
{
    
    public class Pickups
    {
        public enum PickupType {Health, Damage, Speed, Jump}

        public PickupType Type;

        


        public int HealthBonus=3;

        public int JumpBonus=2;

        public int SpeedBonus=2;

        public int DamageBonus=2;

        
        public void SetType(int i)
        {
            switch (i)
            {
                case 0:
                    Type = PickupType.Health;
                    break;
                case 1:
                    Type = PickupType.Damage;
                    break;
                case 2:
                    Type = PickupType.Speed;
                    break;
                case 3:
                    Type = PickupType.Jump;
                    break;
            }

        }

        public int GetType()
        {
            switch(Type)
            {
                case PickupType.Health:
                    return 0;
                case PickupType.Damage:
                    return 1;
                case PickupType.Speed:
                    return 2;
                case PickupType.Jump:
                    return 3;
            }
            return 0;
        }

    }
}
