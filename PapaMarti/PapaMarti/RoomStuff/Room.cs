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
    class Room
    {
        public Tile[,] tiles;
        int height;
        int width;
        Vector2 origin;

        public Vector2 door; //Provides the row and column of where the door is located

        readonly int SCREENWIDTH = Game1.screenRect.Width;
        readonly int SCREENHEIGHT = Game1.screenRect.Height;

        public static readonly int MOVEMENTSPEED = 5;

        public Rectangle borders;

        public List<Vector2> walls; //List of locations where there is a wall

        public List<Vector2> enemySpots; //List of enemy locations in room

        public List<Enemy> enemies;

        public List<Projectile> projectiles;

        int timer;
        KeyboardState oldKB;
        public Room(Tile[,] tiles_, List<Vector2> walls_, List<Vector2> enemySpots_)
        {
            tiles = tiles_;
            walls = walls_;
            enemySpots = enemySpots_;

            height = tiles.GetLength(0);
            width = tiles.GetLength(1);

            //Calculate origin at which to draw room
            int pixelHeight = 60 * height;
            int pixelWidth = 60 * width;
            origin = new Vector2((SCREENWIDTH - pixelWidth) / 2, (SCREENHEIGHT - pixelHeight) / 2);

            door = new Vector2(height / 2, width / 2);

            borders = new Rectangle((int)origin.X, (int)origin.Y, width * 60, height * 60);
            enemies = new List<Enemy>();
            projectiles = new List<Projectile>();
            createEnemies(enemySpots_);

            timer = 0;
            //tiles[(int)door.X, (int)door.Y].status = Status.Door;
            oldKB = Keyboard.GetState();
        }
        public Room(Tile[,] tiles_, List<Vector2> walls_, List<Vector2> enemySpots_, Vector2 door_) : this(tiles_, walls_, enemySpots_)
        {
            door = door_;
            tiles[(int)door.X, (int)door.Y].tilePhysics = TilePhysics.Door;
        }

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

            }
            foreach(Projectile p in projectiles)
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
                foreach (Vector2 v in enemySpots)
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
                foreach (Vector2 v in enemySpots)
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
