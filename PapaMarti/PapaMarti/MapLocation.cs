using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PapaMarti
{
    class MapLocation
    {
        //public Room room; add this back in later when room class is complete
        public double angle;
        public double radius;
        public Texture2D texture;
        public Color color;
        public float scale;
        public Vector2 origin;
        public float rotation;
        public MapLocation(double angle, double radius, Texture2D texture, Color color, float scale, float rotation)
        {
            //this.room = room;
            this.angle = angle;
            this.radius = radius;
            this.texture = texture;
            this.color = color;
            this.scale = scale;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.rotation = rotation;
        }
    }
}
