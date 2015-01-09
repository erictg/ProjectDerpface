using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
namespace ProjectDerpface.Framework.SettingsObjects
{
    public class Controls
    {
        //movement
        public Keys moveForward;
        public Keys moveBackward;
        public Keys moveLeft;
        public Keys moveRight;

        public Keys pause;
        public Keys sound;


        //default contructor
        public Controls()
        {
            moveForward = Keys.W;
            moveBackward = Keys.S;
            moveLeft = Keys.A;
            moveRight = Keys.D;
            pause = Keys.P;
            sound = Keys.Q;
        }

        

       
    }
}
