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
    public abstract class Room
    {
        public Tile[,] tiles;
        int height;
        int width;
        public static int tileSize;
        Vector2 origin;
        public readonly MapLocation location;

        public Vector2 door; //Provides the row and column of where the door is located

        public readonly static int MOVEMENTSPEED = 5;

        public Rectangle borders;

        public List<Vector2> walls; //List of locations where there is a wall

        public List<Vector2> enemySpots; //List of enemy locations in room

        public List<Enemy> enemies;

        public List<Projectile> projectiles;

        int timer;
        Rectangle exit;
        bool exitToMap;

        KeyboardState oldKB;
        public Room(Tile[,] tiles_, List<Vector2> walls_, List<Vector2> enemySpots_, MapLocation location)
        {
            this.location = location;
            tiles = tiles_;
            walls = walls_;
            enemySpots = enemySpots_;

            height = tiles.GetLength(0);
            width = tiles.GetLength(1);
            tileSize = 85;

            //Calculate origin at which to draw room
            int pixelHeight = tileSize * height;
            int pixelWidth = tileSize * width;
            origin = new Vector2((Game1.screenRect.Width - pixelWidth) / 2, (Game1.screenRect.Height - pixelHeight) / 2);
            exit = new Rectangle(0, 0, tileSize, tileSize);
            exit.X += (int)origin.X;
            exit.Y += (int)origin.Y;


            door = new Vector2(height / 2, width / 2);

            borders = new Rectangle((int)origin.X, (int)origin.Y, width * tileSize, height * tileSize);
            enemies = new List<Enemy>();
            projectiles = new List<Projectile>();
            createEnemies(enemySpots_);

            timer = 0;
            //tiles[(int)door.X, (int)door.Y].status = Status.Door;
            oldKB = Keyboard.GetState();
        }
        public Room(Tile[,] tiles_, List<Vector2> walls_, List<Vector2> enemySpots_, Vector2 door_, Vector2 exit, MapLocation location) : this(tiles_, walls_, enemySpots_, location)
        {
            door = door_;

            this.exit = new Rectangle(0, 0, tileSize, tileSize);
            this.exit.X += (int) (origin.X + (exit.X * tileSize));
            this.exit.Y += (int) (origin.Y + (exit.Y * tileSize));
            //Console.WriteLine(origin + ", " + exit + ", " + this.exit + ", " + exit.X * 60 + ", " + exit.Y * 60);

            tiles[(int)door.X, (int)door.Y].tilePhysics = TilePhysics.Door;
        }

        public Room(FiveValuePair<Tile[,], List<Vector2>, Vector2, List<Vector2>, Vector2> data, MapLocation location) : this(data.a, data.b, data.d, data.c, data.e, location) { }

        public void createEnemies(List<Vector2> enemySpots_)
        {
            int x = (int)origin.X;
            int y = (int)origin.Y;
            int counter = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (counter < enemySpots_.Count)
                    {
                        if ((int)enemySpots_[counter].X == i && (int)enemySpots_[counter].Y == j)
                        {
                            enemies.Add(new Mafia(new Rectangle(x, y, 60, 60), 100, 3, 3, 10, 3));
                            counter++;
                        }
                    }





                    x += 60;
                }
                x = (int)origin.X;
                y += 60;
            }
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
                        if (tiles[i, j].tilePhysics == TilePhysics.Wall)
                        {
                        }

                        else
                            spriteBatch.Draw(roomTextures[tiles[i, j].textureid], new Rectangle(x, y, tileSize, tileSize), Color.White);
                    }



                    x += tileSize;
                }
                x = (int)origin.X;
                y += tileSize;
            }


        }

        public Player enter(Player player)
        {
            exitToMap = false;
            player.rect.X = (int)(origin.X + door.Y * 60);
            player.rect.Y = (int)(origin.Y + door.X * 60);
            Console.WriteLine(player.rect + ", " + door);
            player.updateCenter();
            return player;
            //Set player location at the entrance
        }

        public List<Enemy> updateEnemies(Player player)
        {
            timer++;

            bool moveDown = false;
            bool moveRight = false;
            int xMovement = 0;
            int yMovement = 0;
            List<Enemy> remove = new List<Enemy>();
            foreach (Enemy e in enemies)
            {
                /*
                if (player.rect.Intersects(e.rect))
                {
                    if (e.rect.Bottom > player.rect.Center.Y)
                    {
                        yMovement = Rectangle.Intersect(player.rect, e.rect).Height;
                       
                        //e.updateY(Rectangle.Intersect(player.rect, e.rect).Height);
                        //e.bounceOffY();
                    }
                    else if (e.rect.Top < player.rect.Center.Y)
                    {
                        
                        yMovement = -Rectangle.Intersect(player.rect, e.rect).Height;
                        //e.updateY(-Rectangle.Intersect(player.rect, e.rect).Height);
                        //e.bounceOffY();
                    }

                    if (e.rect.Right > player.rect.Center.X)
                    {
                        xMovement = Rectangle.Intersect(player.rect, e.rect).Width;
                        //e.updateX(Rectangle.Intersect(player.rect, e.rect).Width);
                        //e.bounceOffX();
                    }
                    else if (player.rect.Left < player.rect.Center.X)
                    {
                        xMovement = -Rectangle.Intersect(player.rect, e.rect).Width;
                        //moveRight = false;
                        //e.updateX(-Rectangle.Intersect(player.rect, e.rect).Width);
                        //e.bounceOffX();
                    }
                    e.updateX(xMovement);
                    e.updateY(yMovement);
                    
                }
            */
                //e.updateY(e.yVel);



                foreach (Vector2 v in this.walls)
                {

                    if (this.tiles[(int)v.X, (int)v.Y].getRect().Intersects(e.rect))
                    {


                        if (e.rect.Bottom > this.tiles[(int)v.X, (int)v.Y].getRect().Center.Y)
                        {
                            e.updateY(Rectangle.Intersect(this.tiles[(int)v.X, (int)v.Y].getRect(), e.rect).Height);
                            //e.bounceOffY();
                        }
                        else if (e.rect.Top < this.tiles[(int)v.X, (int)v.Y].getRect().Center.Y)
                        {
                            e.updateY(-Rectangle.Intersect(this.tiles[(int)v.X, (int)v.Y].getRect(), e.rect).Height);
                            //e.bounceOffY();
                        }

                    }

                }

                //e.updateX(e.xVel);
                foreach (Vector2 v in this.walls)
                {
                    if (this.tiles[(int)v.X, (int)v.Y].getRect().Intersects(e.rect))
                    {
                        if (e.rect.Right > this.tiles[(int)v.X, (int)v.Y].getRect().Center.X)
                        {
                            e.updateX(Rectangle.Intersect(this.tiles[(int)v.X, (int)v.Y].getRect(), e.rect).Width);
                            //e.bounceOffX();
                        }
                        else if (player.rect.Left < this.tiles[(int)v.X, (int)v.Y].getRect().Center.X)
                        {
                            e.updateX(-Rectangle.Intersect(this.tiles[(int)v.X, (int)v.Y].getRect(), e.rect).Width);
                            //e.bounceOffX();
                        }
                    }

                }

                if (timer % (e.frequency * 60) == 0)
                {
                    projectiles.Add(new Projectile(new Rectangle(e.rect.Center.X, e.rect.Center.Y, 10, 10), (int)e.trajectory(player).X, (int)e.trajectory(player).Y, 20));
                }

                e.takeDamage(player.enemyDamage(e.center));
                if (e.isDead)
                {
                    remove.Add(e);
                }

            }
            foreach (Enemy e in remove)
            {
                enemies.Remove(e);
            }


            foreach (Projectile p in projectiles)
            {
                p.update();
            }
            return enemies;
        }

        public Player update(Player player)
        {
            KeyboardState kb = Keyboard.GetState();
            // TODO: Add your update logic here
            int changeX = 0;
            int changeY = 0;

            //HEALTH SYSTEM TESTING PURPOSES ONLY
            if (oldKB.IsKeyDown(Keys.D) && !kb.IsKeyDown(Keys.D))
            {
                player.takeDamage(20);
            }
            if (oldKB.IsKeyDown(Keys.H) && !kb.IsKeyDown(Keys.H))
            {
                player.heal(10);
            }
            //ends here

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
            player.update(changeX, changeY);

            if (player.rect.Intersects(exit))
                exitToMap = true;
            //Console.WriteLine(player.rect + ", " + exit + ", " + exitToMap);

            oldKB = kb;



            if (player.isDead)
            {
                Environment.Exit(-1);
            }
            return player;
        }
        public List<Projectile> updateProjectiles(Player player)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (player.rect.Intersects(projectiles[i].rect))
                {
                    //Environment.Exit(0);
                    player.takeDamage(projectiles[i].strength);
                    projectiles.RemoveAt(i);
                    i--;
                }

            }
            foreach (Projectile p in projectiles) {
                /*foreach (Vector2 v in this.walls)
                {

                    if (this.tiles[(int)v.X, (int)v.Y].getRect().Intersects(p.rect))
                    {
                            projectiles.Remove(p);
                    }



                }*/
            }
            return projectiles;
        }

        public bool isTouchingDoor()
        {
            if (exitToMap)
            {
                exitToMap = false;
                return true;
            }
            return exitToMap;
        }

        public abstract bool isDone();

        public static Texture2D[] roomTextures = new Texture2D[2];

        public static void initializeTextures(ContentManager content) {
            roomTextures[0] = content.Load<Texture2D>("wood");
            roomTextures[1] = content.Load<Texture2D>("tile");
        }

        public class FiveValuePair<A, B, C, D, E> {
            public A a {
                get; set;
            }

            public B b {
                get; set;
            }

            public C c {
                get; set;
            }

            public D d
            {
                get; set;
            }

            public E e
            {
                get; set;
            }

            public FiveValuePair(A a, B b, C c, D d, E e) {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
                this.e = e;
            }
        }

        public static FiveValuePair<Tile[,], List<Vector2>, Vector2, List<Vector2>, Vector2> parseRoomFile(string file) {
            //foreach(Texture2D t in roomTextures) if(t == null) throw new Exception("Room textures have not been initialized!!");
            List<string> lines = new List<string>();
            List<Vector2> boundaries = new List<Vector2>();
            List<Vector2> enemySpots = new List<Vector2>();
            Vector2 door = new Vector2();
            Vector2 exit = new Vector2();

            try {
                using(StreamReader r = new StreamReader(file)) {
                    while(!r.EndOfStream)
                        lines.Add(r.ReadLine());
                }

                Tile[,] tiles = new Tile[lines.Count, lines[0].Length];

                for(int i = 0; i < tiles.GetLength(0); i++) {
                    char[] tile = lines[i].ToCharArray();

                    for(int j = 0; j < tiles.GetLength(1); j++) {
                        switch(tile[j])
                        {
                            case 'o':
                                tiles[i, j] = new Tile(TilePhysics.Impassable, 1, new Vector2(i, j));
                                boundaries.Add(new Vector2(i, j));
                                break;

                            case 'd':
                                tiles[i, j] = new Tile(TilePhysics.Door, 0, new Vector2(i, j));
                                door = new Vector2(i, j);
                                break;

                            case '.':
                                tiles[i, j] = new Tile(TilePhysics.Passable, 0, new Vector2(i, j));
                                break;

                            case 'b':
                                tiles[i, j] = new Tile(TilePhysics.Wall, 0, new Vector2(i, j));
                                boundaries.Add(new Vector2(i, j));
                                break;

                            case 'e':
                                tiles[i, j] = new Tile(TilePhysics.Passable, 0, new Vector2(i, j));
                                enemySpots.Add(new Vector2(i, j));
                                break;

                            case 'm':
                                tiles[i, j] = new Tile(TilePhysics.Wall, 0, new Vector2(i, j));
                                exit = new Vector2(j, i);
                                break;

                            default:
                                tiles[i, j] = null;
                                break;
                        }
                    }
                }

                return new FiveValuePair<Tile[,], List<Vector2>, Vector2, List<Vector2>, Vector2>(tiles, boundaries, door, enemySpots, exit);
            } catch(Exception e) {
                Console.WriteLine("Error parsing room file " + file);
                Console.WriteLine(e.Message);
                Environment.Exit(-1);
                return null; // This statement will never be reached because of the above statement
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