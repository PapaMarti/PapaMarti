using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    public enum TilePhysics
    {
        Passable, //Can walk through
        Impassable, //Cannot walk through, walls or noninteractable objects
        Collectable //Has an object that can be picked up
    }
    class Tile //Rooms will be made up of Tiles
    {
        public TilePhysics status;
        public Texture2D texture;
        Vector2 location;


        //Null unless tile is Collectable
        public Item item;

        //Rooms will be displayed as a 2d array of tiles. Location refers to the row/column of the tile, not the actual coordinate on the screen.
        //Tiles are 60 x 60
        public Tile(TilePhysics status_, Texture2D texture_, Vector2 location_) //Non-collectable tiles
        {
            status = status_;
            texture = texture_;
            location = location_;
            item = null;
        }

        public Tile(TilePhysics status_, Texture2D texture_, Vector2 location_, Item item_) : this(status_, texture_, location_)
        {
            item = item_;
        }
        
        //Only needed for Collectable tiles
        public Item collect()
        {
            status = TilePhysics.Passable;
            Item itemHolder = item;
            item = null;
            return itemHolder;
        }


    }
}
