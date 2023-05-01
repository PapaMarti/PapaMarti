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
    public class MapLocation {
        public double angle;
        public double radius;
        public Texture2D texture;
        public string texturestring;
        public Color color;
        public float scale;
        public Vector2 origin;
        public float rotation;
        public EmptyRoom emptyQuest {
            get; private set;
        }


        public MapLocation(double angle, double radius, string texturestring, Color color, float scale, float rotation, string roomString)
        {
            //this.room = room;
            this.angle = angle;
            this.radius = radius;
            this.texturestring = texturestring;//g is generic type, b is boss type, p is the pizza shop
            this.color = Color.White;
            this.scale = 5;//standard is 2
            origin = new Vector2();
            this.rotation = rotation;
            emptyQuest = new EmptyRoom(roomString, this);
        }

        public void loadTexture(ContentManager content) {
            texture = content.Load<Texture2D>($"Buildings/{texturestring}");

            origin = new Vector2(texture.Width / 2, texture.Height * 4 / 5);
        }

        public void draw(SpriteBatch spriteBatch, float mapAngle, Vector2 mapPosition) {
            Vector2 location = new Vector2();
            location.X = (float) (mapPosition.X + ((1 - radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation) * Math.Sin(mapAngle - angle));
            location.Y = (float) (mapPosition.Y - ((1 - radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation) * Math.Cos(mapAngle - angle));
            spriteBatch.Draw(texture, location, null, color, 0, origin, scale, SpriteEffects.None, 0f);
        }

        public static bool operator==(MapLocation m1, MapLocation m2) {
            return m1.angle == m2.angle && m1.radius == m2.radius;
        }

        public static bool operator!=(MapLocation m1, MapLocation m2) {
            return !(m1 == m2);
        }
    }
}
