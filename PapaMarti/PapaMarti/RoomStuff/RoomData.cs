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
    class RoomData
    {
        ContentManager content;
        MapLocation[] places;

        Texture2D building;

        //tile textures
        Texture2D floorText;
        Texture2D objectText;

        public RoomData(ContentManager content)
        {
            this.content = content;
            building = content.Load<Texture2D>("whitePixel");

            loadTiles();

            places = new MapLocation[6];

            Texture2D wallTexture = content.Load<Texture2D>("wall");
            //Tile wall = new Tile(TilePhysics.Impassable, wallTexture, new Vector2(0, 0));
            //Tile floo = new Tile(TilePhysics.Passable, content.Load<Texture2D>("whitePixel"), new Vector2(0, 0));
        }

        /// <summary>
        /// Get the location on the map that is closest to the given position
        /// </summary>
        /// <param name="angle">The unit circle angle of the location</param>
        /// <param name="radius">The "up and down" location, 0 for closer to the road, 1 for closer to the inside</param>
        /// <returns></returns>
        

        private void loadTiles()
        {
            floorText = content.Load<Texture2D>("wood");
            objectText = content.Load<Texture2D>("tile");
        }
    }
}
