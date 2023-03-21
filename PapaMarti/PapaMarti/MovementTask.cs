using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    class MovementTask : Task //Player needs to go to a certain location;
    {
        public Vector2 location; //Probably change to Player object later
        public Rectangle target;
        public MovementTask(Vector2 location_, Rectangle target_)
        {
            location = location_;
            target = target_;
        }
        public override void check()
        {
            //add a call to update with updates player location
            if (target.Contains((int)location.X, (int)location.Y))
            {
                taskComplete = true;
            }
        }
        public void update(Vector2 newLocation)
        {
            location = newLocation;
            check();
        }
    }
}
