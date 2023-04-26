using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PapaMarti;

namespace PapaMarti
{
    public enum TilePhysics
    {
        Passable, //Can walk through
        Impassable, //Non-wall, noninteractable objects

        Collectable, //Has an object that can be picked up
        Wall,
        Door
    }
    public class Tile //Rooms will be made up of Tiles
    {
        public TilePhysics tilePhysics;
        public int textureid;
        Vector2 center;
        public int tileSize;

        public Vector2 coordinates;

        //Null unless tile is Collectable
        public Item item;

        //Rooms will be displayed as a 2d array of tiles. Location refers to the row/column of the tile, not the actual coordinate on the screen.
        //Tiles are 60 x 60
        public Tile(TilePhysics tilePhysics_, int textureid, Vector2 coordinates_) //Non-collectable tiles
        {
            tilePhysics = tilePhysics_;
            this.textureid = textureid;
            tileSize = Room.tileSize;
            item = null;

            coordinates = coordinates_;
            center = new Vector2(coordinates.X + (tileSize / 2), coordinates.Y + (tileSize / 2));
        }

        public Tile(TilePhysics tilePhysics_, int textureid, Vector2 coordinates_, Item item_) : this(tilePhysics_, textureid, coordinates_)
        {
            item = item_;
        }

        public void updateCenter()
        {
            center = new Vector2(coordinates.X + (tileSize/2), coordinates.Y + (tileSize / 2));
        }

        //Only needed for Collectable tiles
        public Item collect()
        {
            tilePhysics = TilePhysics.Passable;
            Item itemHolder = item;
            item = null;
            return itemHolder;
        }


        //ONLY FOR SINGLE TILES
        public void draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Room.roomTextures[textureid], getRect(), color);
        }
        public Rectangle getRect() //ONLY FOR SINGLE TILES
        {
            return new Rectangle((int)coordinates.X, (int)coordinates.Y, tileSize, tileSize);
        }
    }
}
