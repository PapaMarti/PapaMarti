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

        public Vector2 door; //Provides the row and column of where the door is located

        readonly int SCREENWIDTH = 1920;
        readonly int SCREENHEIGHT = 1080;

        public Rectangle borders;

        public List<Vector2> walls; //List of locations where there is a wall
        public Room(Tile[,] tiles_, List<Vector2> walls_)
        {
            tiles = tiles_;
            walls = walls_;

            height = tiles.GetLength(0);
            width = tiles.GetLength(1);

            tileSize = 75;

            //Calculate origin at which to draw room
            int pixelHeight = tileSize * height;
            int pixelWidth = tileSize * width;
            origin = new Vector2((SCREENWIDTH - pixelWidth) / 2, (SCREENHEIGHT - pixelHeight) / 2);

            door = new Vector2(height / 2, width - 1);

            borders = new Rectangle((int)origin.X, (int)origin.Y, width * 60, height * 60);
            //tiles[(int)door.X, (int)door.Y].status = Status.Door;
        }
        public Room(Tile[,] tiles_, List<Vector2> walls_, Vector2 door_) : this(tiles_, walls_)
        {
            door = door_;
            tiles[(int)door.X, (int)door.Y].tilePhysics = TilePhysics.Door;
        }

        public void createWalls()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || j == 0 || i == height - 1 || j == width - 1)
                    {
                        //Console.WriteLine(i + "" + j);
                        //Console.WriteLine(tiles[i, j].status);

                        tiles[i, j].tilePhysics = TilePhysics.Wall;
                    }
                }
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            int x = (int)origin.X;
            int y = (int)origin.Y;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tiles[i, j].coordinates = new Vector2(x, y);
                    if (tiles[i, j].tilePhysics == TilePhysics.Wall)

                        spriteBatch.Draw(tiles[i, j].texture, new Rectangle(x, y, 60, 60), Color.Black);
                    else
                        spriteBatch.Draw(tiles[i, j].texture, new Rectangle(x, y, 60, 60), Color.White);


                    x += 60;
                }
                x = (int)origin.X;
                y += tileSize;
            }
        }
        public Player enter(Player player)
        {
            player.rect.X = (int)(origin.X + door.Y * 60);
            player.rect.Y = (int)(origin.Y + door.X * 60);
            return player;
            //Set player location at the entrance
        }

        /*
         player.rect = player.update(changeX, changeY);
            foreach (Vector2 v in room.walls) 
            {
                
                if (room.tiles[(int)v.X, (int)v.Y].getRect().Intersects(player.rect))
                {
                    
                    Rectangle r = Rectangle.Intersect(room.tiles[(int)v.X, (int)v.Y].getRect(), player.rect);
                    int xAxis = r.Width;
                    int yAxis = r.Height;
                    Console.WriteLine(yAxis);
                    if (xAxis < yAxis)
                    {
                        player.updateX(-changeX);
                    }
                    else if (yAxis < xAxis) 
                    {
                        player.updateY(-changeY);
                    }
                    else if (xAxis == yAxis)
                    {
                        player.rect = player.update(-changeX, -changeY);
                    }
                    
                }
                
            }
         * */
    }
}
