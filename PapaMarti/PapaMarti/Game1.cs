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

namespace PapaMarti {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        public static Rectangle screenRect = new Rectangle(0, 0, 1920, 1080);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StageManager currentStage;
        MapManager mapManager;
        Quest currentQuest;
        RoomData data;

        Texture2D ovenText;
        Texture2D texture;
        Texture2D playerText;
        Texture2D woodText;
        Room room;




        Player player;

        readonly int MOVEMENTSPEED = 5;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; 
            graphics.PreferredBackBufferWidth = screenRect.Width;
            graphics.PreferredBackBufferHeight = screenRect.Height;
            graphics.IsFullScreen = true;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D baseRect = new Texture2D(GraphicsDevice, 1, 1);
            baseRect.SetData(new Color[] { Color.White });
            currentStage = new CookingManager(GraphicsDevice, Content, baseRect, new Pizza(PizzaShape.Circle, new List<Rectangle>(), new List<Topping>(), 10), true);

            data = new RoomData(Content);

            Task[] list = new Task[0];
            currentQuest = new Quest(list, 0.55, 0.8);
            mapManager = new MapManager(Content, 0, 0, currentQuest, 5, data, true);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        private Room generateRoom(string path, Texture2D floorText, Texture2D objectText)
        {
            List<string> lines = new List<string>();
            bool hasDoor = false;
            List<Vector2> boundaries = new List<Vector2>();
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
                    Room r = new Room(tiles, boundaries);
                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        char[] tile = lines[i].ToCharArray();

                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {

                            if (tile[j] == 'o')
                            {
                                tiles[i, j] = new Tile(TilePhysics.Impassable, objectText, new Vector2(i, j));
                                boundaries.Add(new Vector2(i, j));

                            }
                            else if (tile[j] == 'd')
                            {
                                tiles[i, j] = new Tile(TilePhysics.Door, floorText, new Vector2(i, j));
                                hasDoor = true;
                                door = new Vector2(i, j);
                            }
                            else if (tile[j] == '.')
                            {
                                tiles[i, j] = new Tile(TilePhysics.Passable, floorText, new Vector2(i, j));
                            }
                            else
                            {
                                tiles[i, j] = new Tile(TilePhysics.Wall, floorText, new Vector2(i, j));
                                boundaries.Add(new Vector2(i, j));

                            }
                        }
                    }

                    if (hasDoor)
                    {
                        r = new Room(tiles, boundaries, door);

                        return r;
                    }
                    else
                    {
                        r = new Room(tiles, boundaries);
                        return r;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
            return new Room(new Tile[0, 0], boundaries);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if(currentStage.getStage() == GameStage.Cooking && currentStage.isDone())
            {
                currentStage = mapManager;
            }

            if(currentStage.getStage() == GameStage.Exploring)
            {
                if (currentStage.isDone())
                {
                    currentStage = new RoomManager(Content, ((MapManager)currentStage).closestLocation);
                }
            }

            if(currentStage.getStage() == GameStage.Rooming)
            {
                if (currentStage.isDone())
                {
                    currentStage = mapManager;
                }
            }
            player = room.update(player);
            // TODO: Add your update logic here
            currentStage.update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            room.draw(spriteBatch);
            player.draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
