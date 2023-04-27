using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PapaMarti
{
    public class Circle
    {
        public double radius;
        public Vector2 location;

        public Circle(double radius)
        {
            this.radius = radius;
            location = new Vector2(0, 0);
        }
        public Circle(double radius, Vector2 location)
        {
            this.radius = radius;
            this.location = location;
        }

        public bool isInCircle(Vector2 point)
        {
            return radius * radius > Math.Pow(point.X - location.X, 2) + Math.Pow(point.Y - location.Y, 2);
        }
    }
}
