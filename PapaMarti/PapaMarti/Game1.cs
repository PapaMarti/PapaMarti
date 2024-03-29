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
        Menu menu;
        bool isInMenu;
        Player player;
        KeyboardState oldKB;

        public static Color shaded = new Color(0.2f, 0.2f, 0.2f, 0.5f);

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
            isInMenu = false;
            oldKB = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Room.initializeTextures(Content);

            menu = new Menu(this, Content, true);

            player = new Player(Content, new Rectangle(1800, 500, 60, 60), 300);

            mapManager = new MapManager(Content, 0, 0, 1, true);

            QuestTracker.initializeTextures(Content, player, mapManager);

            //UNCOMMENT THIS TO GO DIRECTLY TO THE MAP
            currentStage = mapManager;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit in case of emergency
            //if(kb.IsKeyDown(Keys.L))
            //    this.Exit();


            if (isInMenu && menu.isDone())
            {
                isInMenu = false;
            }
            if (kb.IsKeyDown(Keys.Escape) && !isInMenu)
            {
                isInMenu = true;
                menu.reset();
            }

            if(currentStage.getStage() == GameStage.Cooking && currentStage.isDone())
            {
                currentStage = mapManager;
            }

            if(currentStage.getStage() == GameStage.Exploring)
            {

                if (currentStage.isDone())
                {
                    currentStage = QuestTracker.enterRoom(Content, player, ((MapManager) currentStage).closestLocation);
                }
            }

            if(currentStage.getStage() == GameStage.Rooming)
            {
                if (currentStage.isDone())
                {
                    currentStage = mapManager;
                }
            }
            // TODO: Add your update logic here

            if (!isInMenu)
                currentStage.update(gameTime);
            else
                menu.update();
            testAddWeapon();
            oldKB = kb;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            Color backgroundColor = Color.MediumBlue;
            if (currentStage.getStage() == GameStage.Rooming)
                backgroundColor = Color.Black;

            GraphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            currentStage.draw(spriteBatch);

            if (isInMenu)
                menu.draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void testAddWeapon()
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Z) && oldKB.IsKeyUp(Keys.Z))
            {
                ((PizzaFrisbee)player.weapons[0]).upgrade();
            }
            else if (kb.IsKeyDown(Keys.X) && oldKB.IsKeyUp(Keys.X))
            {
                player.addWeapon(new PizzaFrisbee(Content, player, WeaponType.Throw));
            }
            else if (kb.IsKeyDown(Keys.C) && oldKB.IsKeyUp(Keys.C))
            {
                player.addWeapon(new Bomb(Content, player, WeaponType.Bomb));
            }
        }
    }
}
