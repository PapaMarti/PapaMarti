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

        public Vector2 door; //Provides the row and column of where the door is located

        readonly int SCREENWIDTH = 1920;
        readonly int SCREENHEIGHT = 1080;

        public Rectangle borders;


        public Room(Tile[,] tiles_)
        {
            tiles = tiles_;

            height = tiles.GetLength(0);
            width = tiles.GetLength(1);

            //Calculate origin at which to draw room
            int pixelHeight = 60 * height;
            int pixelWidth = 60 * width;
            origin = new Vector2((SCREENWIDTH - pixelWidth) / 2, (SCREENHEIGHT - pixelHeight) / 2);

            door = new Vector2(height / 2, width - 1);

            borders = new Rectangle((int)origin.X, (int)origin.Y, width * 60, height * 60);
            //tiles[(int)door.X, (int)door.Y].status = Status.Door;
        }
        public Room(Tile[,] tiles_, Vector2 door_) : this(tiles_)
        {
            door = door_;
            tiles[(int)door.X, (int)door.Y].status = Status.Door;
        }
        public void draw(SpriteBatch spriteBatch)
        {
            int x = (int)origin.X;
            int y = (int)origin.Y;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    spriteBatch.Draw(tiles[i, j].texture, new Rectangle(x, y, 60, 60), Color.White);

                    x += 60;
                }
                x = (int)origin.X;
                y += 60;
            }
        }
        public Player enter(Player player)
        {
            player.rect.X = (int)(origin.X + door.Y * 60);
            player.rect.Y = (int)(origin.Y + door.X * 60);
            return player;
            //Set player location at the entrance
        }

        //METHOD TO GENERATE ROOMS FROM A TEXT FILE
        //MOVE THIS SOMEWHERE ELSE


        /*
         * private Room generateRoom(string path, Texture2D floorText, Texture2D objectText)
        {
            List<string> lines = new List<string>();
            bool hasDoor = false;
            Vector2 door = new Vector2(0, 0);
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    //Sort all text into lines
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lines.Add(line);
                    }

                    Tile[,] tiles = new Tile[lines.Count, lines[0].Length];

                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        char[] tile = lines[i].ToCharArray();

                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {

                            if (tile[j] == 'o')
                            {
                                tiles[i, j] = new Tile(Status.Impassable, objectText, new Vector2(i, j));

                            }
                            else if (tile[j] == 'd')
                            {
                                tiles[i, j] = new Tile(Status.Door, floorText, new Vector2(i, j));
                                hasDoor = true;
                                door = new Vector2(i, j);
                            }
                            else
                            {
                                tiles[i, j] = new Tile(Status.Passable, floorText, new Vector2(i, j));
                            }
                        }
                    }

                    if (hasDoor)
                    {
                        Room r = new Room(tiles, door);
                        return r;
                    }
                    else
                    {
                        Room r = new Room(tiles);
                        return r;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
            return new Room(new Tile[0, 0]);
        }
         */
    }
}
