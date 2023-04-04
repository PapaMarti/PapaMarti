using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    class Room
    {
        public Tile[,] tiles;
        int height;
        int width;
        Vector2 origin;
        int tileSize;


        readonly int SCREENWIDTH = 1920;
        readonly int SCREENHEIGHT = 1080;

        public Room(Tile[,] tiles_)
        {
            tiles = tiles_;
            height = tiles.GetLength(0);
            width = tiles.GetLength(1);

            tileSize = 75;

            //Calculate origin at which to draw room
            int pixelHeight = tileSize * height;
            int pixelWidth = tileSize * width;
            origin = new Vector2((SCREENWIDTH - pixelWidth) / 2, (SCREENHEIGHT - pixelHeight) / 2);
        }
        public void draw(SpriteBatch spriteBatch)
        {
            int x = (int)origin.X;
            int y = (int)origin.Y;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if(tiles[i, j] != null)
                        spriteBatch.Draw(tiles[i, j].texture, new Rectangle(x, y, tileSize, tileSize), Color.White);
                    x += tileSize;
                }
                x = (int)origin.X;
                y += tileSize;
            }
        }
    }
}
