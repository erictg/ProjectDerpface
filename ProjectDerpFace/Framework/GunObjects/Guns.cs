using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ProjectDerpface.Framework.GunObjects
{
    public class Gun
    {
        //gun attributes
        public string gunName;
        public string caliberName;
        public float caliberNumber;
        public Bullet bulletType;
        public int clipSize;
        
        //to add later - stuff to attatch the gun to the player object
        //camera offset for the gun
        //zoom scope of the gun

        public Gun(string gunName, Bullet bulletType, int clipSize)
        {
            this.gunName = gunName;
            caliberName = bulletType.caliberName;
            caliberNumber = bulletType.caliberNumber;
        }
    }
    public static class GunTypes
    {
        //contains preset guns
    }
}
