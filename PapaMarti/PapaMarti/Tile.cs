using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testing;

namespace PapaMarti
{
    public enum Status
    {
        Passable, //Can walk through
        Impassable, //Non-wall, noninteractable objects

        Collectable, //Has an object that can be picked up

        Door
    }
    class Tile //Rooms will be made up of Tiles
    {
        public Status status;
        public Texture2D texture;
        Vector2 location;

        Vector2 coordinates; //Only for single tiles(doors)

        //Null unless tile is Collectable
        public Item item;

        //Rooms will be displayed as a 2d array of tiles. Location refers to the row/column of the tile, not the actual coordinate on the screen.
        //Tiles are 60 x 60
        public Tile(Status status_, Texture2D texture_, Vector2 location_) //Non-collectable tiles
        {
            status = status_;
            texture = texture_;
            location = location_;


            item = null;

            coordinates = new Vector2(-300, -300);
        }

        public Tile(Status status_, Texture2D texture_, Vector2 location_, Item item_) : this(status_, texture_, location_)
        {
            item = item_;
            coordinates = new Vector2(-300, -300);
        }
        public Tile(Texture2D texture_, Vector2 coordinates_) //For doors only
        {
            status = Status.Door;
            texture = texture_;
            coordinates = coordinates_;
        }

        //Only needed for Collectable tiles
        public Item collect()
        {
            status = Status.Passable;
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
