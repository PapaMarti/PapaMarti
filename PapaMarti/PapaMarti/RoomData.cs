﻿using Microsoft.Xna.Framework;
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

            Texture2D wallTexture = content.Load<Texture2D>("wall");
            Tile wall = new Tile(TilePhysics.Impassable, wallTexture, new Vector2(0, 0));
            Tile floor = new Tile(TilePhysics.Passable, content.Load<Texture2D>("whitePixel"), new Vector2(0, 0));

            //slice one
            Room one = new Room(new Tile[,] {{wall, wall, wall, wall, wall, wall, wall, wall, wall },
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             { wall, wall, wall, wall, wall, wall, wall, wall, wall } });
            Room two = new Room(new Tile[,] {{wall, wall, wall, wall, wall, wall, wall, wall, wall, wall },
                                             {wall, floor, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, wall, wall, wall, wall, wall, floor, floor, floor, wall},
                                             {null, null, null, null, null, wall, floor, floor, floor, wall},
                                             {null, null, null, null, null, wall, floor, floor, floor, wall},
                                             {null, null, null, null, null, wall, floor, floor, floor, wall},
                                             {null, null, null, null, null, wall, floor, floor, floor, wall},
                                             {null, null, null, null, null, wall, wall, wall, wall, wall } });
            Room three = new Room(new Tile[,] {{null, null, wall, wall, wall, wall, wall, null, null },
                                             {null, null, wall, floor, floor, floor, wall, null, null},
                                             {wall, wall, wall, floor, floor, floor, wall, wall, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, floor, floor, floor, floor, floor, floor, floor, wall},
                                             {wall, wall, wall, floor, floor, floor, wall, wall, wall},
                                             {null, null, wall, floor, floor, floor, wall, null, null},
                                             {null, null, wall, wall, wall, wall, wall, null, null } });
            places[0] = new MapLocation(0.4, 0.78, building, Color.Blue, 1.1f, 0f, one);
            places[1] = new MapLocation(0.3, 0.47, building, Color.Red, 1f, 0f, two);
            places[2] = new MapLocation(0.15, 0.22, building, Color.Yellow, 1.2f, 0f, three);
            places[3] = new MapLocation(0.36, 0.15, building, Color.Green, 0.9f, 0.25f, one);
            places[4] = new MapLocation(0.8, 0.12, building, Color.Purple, 0.8f, -0.2f, two);
            places[5] = new MapLocation(0.8, 0.55, building, Color.Orange, 1f, 0f, three);
        }
        public void drawLocations(SpriteBatch spriteBatch, float angle, Vector2 mapPosition)
        {
            Vector2 location = new Vector2(0, 0);
            for(int i = 0; i < places.Length; i++)
            {
                location.X = (float)(mapPosition.X + ((1 - places[i].radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation) * Math.Sin(angle - places[i].angle));
                location.Y = (float)(mapPosition.Y - ((1 - places[i].radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation) * Math.Cos(angle - places[i].angle));
                spriteBatch.Draw(places[i].texture, location, null, places[i].color, 0, places[i].origin, places[i].scale, SpriteEffects.None, 0f);
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