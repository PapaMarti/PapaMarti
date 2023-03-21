using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    class MovementTask : Task //Player needs to go to a certain location;
    {
        public double radius; //Probably change to Player object later
        public double angle;
        public Rectangle target;
        public MovementTask(double radius_, double angle_, Rectangle target_)
        {
            radius = radius_;
            angle = angle_;
            target = target_;
        }
        public override void check()
        {
            //add a call to update with updated player location
            if (target.Contains((int)(radius * Math.Cos(angle)), (int)(radius * Math.Sin(angle))))
            {
                taskComplete = true;
            }
        }
        public void update(double newRadius, double newAngle)
        {
            radius = newRadius;
            angle = newAngle;
            check();
        }
    }
}
