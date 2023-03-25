using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
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
        Quest currentQuest;
        

        Texture2D ovenText;

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
            //currentStage = new CookingManager(Content, baseRect, new Pizza(PizzaShape.Circle, new List<Rectangle>(), new List<Topping>(), 0));
            Task[] list = new Task[0];
            currentQuest = new Quest(list, 0.5, 11 * Math.PI / 6);
            currentStage = new MapManager(Content, 0, 0, currentQuest, 1);
            // TODO: use this.Content to load your game content here

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
            // Allows the game to exit
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

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
            currentStage.draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
