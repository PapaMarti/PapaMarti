using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace PapaMarti
{
    public abstract class Room
    {
        public Tile[,] tiles;
        int height;
        int width;
        Vector2 origin;
        public readonly MapLocation location;

        public Vector2 door; //Provides the row and column of where the door is located

        readonly int MOVEMENTSPEED = 5;

        public Rectangle borders;

        public List<Vector2> walls; //List of locations where there is a wall
        public Room(Tile[,] tiles_, List<Vector2> walls_, MapLocation location)
        {
            this.location = location;
            tiles = tiles_;
            walls = walls_;

            height = tiles.GetLength(0);
            width = tiles.GetLength(1);

            //Calculate origin at which to draw room
            int pixelHeight = 60 * height;
            int pixelWidth = 60 * width;
            origin = new Vector2((Game1.screenRect.Width - pixelWidth) / 2, (Game1.screenRect.Height - pixelHeight) / 2);

            door = new Vector2(height / 2, width / 2);

            borders = new Rectangle((int)origin.X, (int)origin.Y, width * 60, height * 60);
            //tiles[(int)door.X, (int)door.Y].status = Status.Door;
        }
        public Room(Tile[,] tiles_, List<Vector2> walls_, Vector2 door_, MapLocation location) : this(tiles_, walls_, location)
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
                    if (tiles[i, j] != null)
                    {
                        tiles[i, j].coordinates = new Vector2(x, y);
                        if (tiles[i, j].tilePhysics == TilePhysics.Wall) {
                        }

                            //spriteBatch.Draw(tiles[i, j].texture, new Rectangle(x, y, 60, 60), Color.Black);
                        else
                            spriteBatch.Draw(tiles[i, j].texture, new Rectangle(x, y, 60, 60), Color.White);
                    }



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
        public Player update(Player player)
        {
            KeyboardState kb = Keyboard.GetState();
            // TODO: Add your update logic here
            int changeX = 0;
            int changeY = 0;

            if (kb.IsKeyDown(Keys.Right))
            {
                changeX += MOVEMENTSPEED;
            }
            if (kb.IsKeyDown(Keys.Left))
            {
                changeX -= MOVEMENTSPEED;
            }
            if (kb.IsKeyDown(Keys.Up))
            {
                changeY -= MOVEMENTSPEED;
            }
            if (kb.IsKeyDown(Keys.Down))
            {
                changeY += MOVEMENTSPEED;
            }


            if (changeY != 0)
            {
                player.updateY(changeY);

                foreach (Vector2 v in this.walls)
                {

                    if (this.tiles[(int)v.X, (int)v.Y].getRect().Intersects(player.rect))
                    {


                        if (player.rect.Bottom > this.tiles[(int)v.X, (int)v.Y].getRect().Center.Y)
                        {
                            player.updateY(-changeY);
                        }
                        else if (player.rect.Top < this.tiles[(int)v.X, (int)v.Y].getRect().Center.Y)
                        {
                            player.updateY(-changeY);
                        }

                    }

                }
            }

            if (changeX != 0)
            {
                player.updateX(changeX);
                foreach (Vector2 v in this.walls)
                {
                    if (this.tiles[(int)v.X, (int)v.Y].getRect().Intersects(player.rect))
                    {
                        if (player.rect.Right > this.tiles[(int)v.X, (int)v.Y].getRect().Center.X)
                        {
                            player.updateX(-changeX);
                        }
                        else if (player.rect.Left < this.tiles[(int)v.X, (int)v.Y].getRect().Center.X)
                        {
                            player.updateX(-changeX);
                        }
                    }

                }

            }
            return player;
        }

        public abstract bool isDone();

        public static Texture2D[] roomTextures = new Texture2D[2];

        public static void initializeTextures(ContentManager content) {
            roomTextures[0] = content.Load<Texture2D>("wood");
            roomTextures[1] = content.Load<Texture2D>("tile");
        }

        struct ThreeValuePair<A, B, C> {
            A a;
            B b;
            C c;
        }

        public static ThreeValuePair<Tile[,], List<Vector2>, Vector2> parseRoomFile(string file) {
            List<string> lines = new List<string>();
            bool hasDoor = false;
            List<Vector2> boundaries = new List<Vector2>();
            Vector2 door = new Vector2(0, 0);

            try {
                using(StreamReader r = new StreamReader(file)) {
                    while(!r.EndOfStream)
                        lines.Add(r.ReadLine());
                }

                Tile[,] tiles = new Tile[lines.Count, lines[0].Length];

                for(int i = 0; i < tiles.GetLength(0); i++) {
                    char[] tile = lines[i].ToCharArray();

                    for(int j = 0; j < tiles.GetLength(1); j++) {
                        if(tile[j] == 'o') {
                            tiles[i, j] = new Tile(TilePhysics.Impassable, roomTextures[1], new Vector2(i, j));
                            boundaries.Add(new Vector2(i, j));

                        }
                        else if(tile[j] == 'd') {
                            tiles[i, j] = new Tile(TilePhysics.Door, roomTextures[0], new Vector2(i, j));
                            hasDoor = true;
                            door = new Vector2(i, j);
                        }
                        else if(tile[j] == '.') {
                            tiles[i, j] = new Tile(TilePhysics.Passable, roomTextures[0], new Vector2(i, j));
                        }
                        else if(tile[j] == 'b') {
                            tiles[i, j] = new Tile(TilePhysics.Wall, roomTextures[0], new Vector2(i, j));
                            boundaries.Add(new Vector2(i, j));

                        }
                        else {
                            tiles[i, j] = null;
                        }
                    }
                }

                if(hasDoor) 
            } catch(Exception e) {
                Console.WriteLine("Error parsing room file " + file);
                Console.WriteLine(e.Message);
                Environment.Exit(-1);
            }
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
