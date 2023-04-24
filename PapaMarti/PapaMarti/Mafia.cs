using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    //MAXSPEED of mafia is 
    class Mafia : Enemy
    {
        public Mafia(Rectangle rect_, int maxLife_, int maxXVel_, int maxYVel_) : base(rect_, maxLife_, maxXVel_, maxYVel_)
        {

        }
        public override void homing(Player p)
        {
            int distanceX = (int)(p.center.X - this.center.X);
            int distanceY = (int)(p.center.Y - this.center.Y);

            throw new NotImplementedException();
        }
    }
}
