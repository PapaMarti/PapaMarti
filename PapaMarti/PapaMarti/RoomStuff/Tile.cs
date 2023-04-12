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
    class Tile //Rooms will be made up of Tiles
    {
        public TilePhysics tilePhysics;
        public Texture2D texture;
        Vector2 center;

        public Vector2 coordinates;

        //Null unless tile is Collectable
        public Item item;

        //Rooms will be displayed as a 2d array of tiles. Location refers to the row/column of the tile, not the actual coordinate on the screen.
        //Tiles are 60 x 60
        public Tile(TilePhysics tilePhysics_, Texture2D texture_, Vector2 coordinates_) //Non-collectable tiles
        {
            tilePhysics = tilePhysics_;
            texture = texture_;


            item = null;

            coordinates = coordinates_;
            center = new Vector2(coordinates.X + 30, coordinates.Y + 30);
        }

        public Tile(TilePhysics tilePhysics_, Texture2D texture_, Vector2 coordinates_, Item item_) : this(tilePhysics_, texture_, coordinates_)
        {
            item = item_;
        }

        public void updateCenter()
        {
            center = new Vector2(coordinates.X + 30, coordinates.Y + 30);
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
            spriteBatch.Draw(texture, getRect(), color);
        }
        public Rectangle getRect() //ONLY FOR SINGLE TILES
        {
            return new Rectangle((int)coordinates.X, (int)coordinates.Y, 60, 60);
        }
    }
}
