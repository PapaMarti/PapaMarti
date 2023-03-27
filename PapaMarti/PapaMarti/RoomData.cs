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
    class RoomData
    {
        ContentManager content;
        MapLocation[] places;

        Texture2D building;
        public RoomData(ContentManager content)
        {
            this.content = content;
            building = content.Load<Texture2D>("whitePixel");

            places = new MapLocation[6];

            //slice one
            places[0] = new MapLocation(0.4, 0.8, building, Color.Blue, 1f);
            places[1] = new MapLocation(0.3, 0.45, building, Color.Red, 1f);
            places[2] = new MapLocation(0.15, 0.2, building, Color.Yellow, 1f);
            places[3] = new MapLocation(0.4, 0.12, building, Color.Green, 1f);
            places[4] = new MapLocation(0.8, 0.12, building, Color.Purple, 1f);
            places[5] = new MapLocation(0.8, 0.55, building, Color.Orange, 1f);
        }
        public void drawLocations(SpriteBatch spriteBatch, float angle, Vector2 mapPosition)
        {
            Vector2 location = new Vector2(0, 0);
            for(int i = 0; i < places.Length; i++)
            {
                location.X = (float)(mapPosition.X + ((1 - places[i].radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation) * Math.Sin(angle - places[i].angle));
                location.Y = (float)(mapPosition.Y - ((1 - places[i].radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation) * Math.Cos(angle - places[i].angle));
                spriteBatch.Draw(places[i].texture, location, null, places[i].color, angle, places[i].origin, places[i].scale, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Get the location on the map that is closest to the given position
        /// </summary>
        /// <param name="angle">The unit circle angle of the location</param>
        /// <param name="radius">The "up and down" location, 0 for closer to the road, 1 for closer to the inside</param>
        /// <returns></returns>
        public MapLocation getClosestLocation(double angle, double radius)
        {
            double r = (1 - radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation;
            MapLocation closest = places[0];
            double minDistance = int.MaxValue;
            for(int i = 0; i < places.Length; i++)
            {
                double placeRadius = (1 - places[i].radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation;
                double distance = Math.Sqrt(Math.Pow(r, 2) + Math.Pow(placeRadius, 2) - 2 * r * placeRadius * Math.Cos(angle - places[i].angle));
                if(distance < minDistance)
                {
                    closest = places[i];
                    minDistance = distance;
                }
            }
            return closest;
        }
    }
}
