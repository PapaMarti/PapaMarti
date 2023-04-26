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
        public Mafia(Rectangle rect_, int maxLife_, int maxXVel_, int maxYVel_, int fireSpeed_, int frequency_) : base(rect_, maxLife_, maxXVel_, maxYVel_, fireSpeed_, frequency_)
        {
            xVel = 0;
            yVel = 0;
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

        public override Vector2 trajectory(Player p)
        {
            int xDist = p.rect.Center.X - this.rect.Center.X;
            int yDist = p.rect.Center.Y - this.rect.Center.Y;
            int xFactor = xDist / Math.Abs(xDist);
            int yFactor = yDist / Math.Abs(yDist);
            double theta = Math.Abs(Math.Atan((yDist * 1.0) / xDist));
            //Console.WriteLine(Math.Cos(theta));
            //Console.WriteLine(Math.Sin(theta));
            return new Vector2((float)(fireSpeed * Math.Cos(theta) * xFactor), (float)(fireSpeed * Math.Sin(theta) * yFactor));
        }
    }
}
