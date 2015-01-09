using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUutilities;
namespace ProjectDerpface.Framework.GunObjects
{
    //public class Bullet : EntityModel
    public class Bullet
    {
        public string caliberName;
        public float caliberNumber;
        public float currentEnergy;
        public float maxEnergy;
        public float maxDamage;
        
        
        public Bullet()
            :base()
        {
           
            

        }
         
        public float determineDammage()
        {
            /*
             * currentEnergy     inflictedDamage
             * -------------  = ----------------
             * maxEnergy         maxDamage
             */
            return (currentEnergy * maxDamage) / maxEnergy;
        }

        public bool checkBulletPass(float otherEnergyToPass)
        {
            return (currentEnergy > otherEnergyToPass);
        }
    }
}
