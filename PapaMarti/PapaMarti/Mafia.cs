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
            xVel = maxXVel;
            yVel = maxYVel;
        }
        private double baseComponent(double velocity)
        {
            double final = Math.Pow(velocity, 2);
            final /= 2;
            return Math.Sqrt(final);
        }
        public override void bounceOffX()
        {
            xVel *= -1;
            /*
            if (!p.rect.Intersects(this.rect))
            {
                
                int distanceX = (int)(p.center.X - this.center.X);
                int distanceY = (int)(p.center.Y - this.center.Y);
                double ratio = Math.Abs(distanceY * 1.0 / distanceX);

                //temp set all velocities to max
                /*if (distanceX < 0)
                {
                    xVel = -maxVel;
                }
                else if (distanceX > 0)
                {
                    xVel = 1;
                }
                else
                {
                    xVel = 0;
                }
                if (distanceX != 0)
                {
                    slope = Math.Abs(distanceY * 1.0 / distanceX);
                    if (slope > maxYVel)
                        slope = maxYVel;
                    if (distanceY < 0)
                        yVel = -(int)slope;
                    else
                        yVel = (int)slope;
                }
                else
                {
                    if (distanceY < 0)
                    {
                        yVel = -maxYVel;
                    }
                    else
                    {
                        yVel = maxYVel;
                    }
                }

                
                /*if (distanceY < 0)
                {
                    yVel = -maxYVel;
                }
                else
                {
                    yVel = maxYVel;
                }*/
            
            
        }
        public override void bounceOffY()
        {
            yVel *= -1;
        }

        public override void trajectory(Player p)
        {
            throw new NotImplementedException();
        }
    }
}
