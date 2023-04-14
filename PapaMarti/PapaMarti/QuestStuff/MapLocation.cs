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
    public class MapLocation
    {
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

        public void draw(SpriteBatch spriteBatch, float mapAngle, Vector2 mapPosition) {
            Vector2 location = new Vector2();
            location.X = (float) (mapPosition.X + ((1 - radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation) * Math.Sin(mapAngle - angle));
            location.Y = (float) (mapPosition.Y - ((1 - radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation) * Math.Cos(mapAngle - angle));
            spriteBatch.Draw(texture, location, null, color, mapAngle + rotation, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
