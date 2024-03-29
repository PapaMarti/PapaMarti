﻿using Microsoft.Xna.Framework;
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

    public class CookingManager : StageManager {
        private readonly Pizza type;
        public CookingStage currentStage;
        private Texture2D baseRect;
        private GraphicsDevice gd;
        private bool isTransitioning;
        private bool isFadingIn, isFadingOut;
        private Color alpha;
        public double accuracy; //a number from 0.0 to 1.0
        private int waitTime; //in 1/60th of a seconds
        private bool hasWaited;
        SpriteFont font;
        bool drawTable;
        Texture2D table;
        Rectangle retryButton;
        bool done;
        bool readyToMoveOn;
        Texture2D pixel;
        Texture2D backgroundYes;

        public CookingManager(GraphicsDevice gd, ContentManager content, Texture2D baseRect, Pizza type) : base(content) {
            this.gd = gd;
            this.type = type;
            isTransitioning = false;
            isFadingIn = false;
            isFadingOut = false;
            this.baseRect = baseRect;
            int retryWidth = Game1.screenRect.Width / 10;
            int retryHeight = Game1.screenRect.Height / 10;
            retryButton = new Rectangle((Game1.screenRect.Width - retryWidth) / 2, (Game1.screenRect.Height - retryHeight) / 2 + 40, retryWidth, retryHeight);
            alpha = new Color(0, 0, 0, 0);
            accuracy = 0.0;
            waitTime = 0;
            hasWaited = false;
            drawTable = true;
            readyToMoveOn = false;
            table = content.Load<Texture2D>("CookingStageTextures/Table");
            font = content.Load<SpriteFont>("text01");
            pixel = content.Load<Texture2D>("whitePixel");
            backgroundYes = content.Load<Texture2D>("CookingStageTextures/OvenTextures/Ovenbg");
            if(type.shape == PizzaShape.Circle)
                currentStage = new CuttingScreen(type, Game1.screenRect, content.Load<Texture2D>("CookingStageTextures/CuttingStageTextures/dough"), content.Load<Texture2D>("CookingStageTextures/CuttingStageTextures/circle outline"), content.Load<Texture2D>("CookingStageTextures/circle dough"), content.Load<Texture2D>("whitePixel"));
        }

        public override void draw(SpriteBatch spriteBatch) {
            if (drawTable)
                spriteBatch.Draw(table, Game1.screenRect, Color.White);
            else
                spriteBatch.Draw(backgroundYes, Game1.screenRect, new Rectangle(0,0,240,135), Color.White);

            currentStage.draw(spriteBatch);
            if(isTransitioning) {
                spriteBatch.Draw(baseRect, Game1.screenRect, alpha);
            }
            if(waitTime > 0 && !done)
            {
                string accuracyText = "Accuracy: " + Math.Round(currentStage.getAccuracy() * 100) + "%";
                spriteBatch.DrawString(font, accuracyText, new Vector2((Game1.screenRect.Width - font.MeasureString(accuracyText).X) / 2, (Game1.screenRect.Height - font.MeasureString(accuracyText).Y) / 2), Color.Black);
            }
            if(done && !readyToMoveOn)
            {
                string accuracyText = "Total Accuracy: " + Math.Round(accuracy * 100) + "%";
                spriteBatch.DrawString(font, accuracyText, new Vector2((Game1.screenRect.Width - font.MeasureString(accuracyText).X) / 2, (Game1.screenRect.Height - font.MeasureString(accuracyText).Y) / 2), Color.Black);
                spriteBatch.Draw(pixel, retryButton, Color.Blue);
            }
        }

        public override GameStage getStage() {
            return GameStage.Cooking;
        }

        public override bool isDone() {
            return readyToMoveOn;
        }

        public override void update(GameTime time) {
            currentStage.update(time);
            MouseState mouse = Mouse.GetState();
            waitTime--;
            if(currentStage.getStage() == CookStage.Cooking && currentStage.isDone() && waitTime <= 0)
            {
                done = true;
            }
            if(done && waitTime > 0)
            {
                if(mouse.LeftButton == ButtonState.Pressed && retryButton.Contains(new Point(mouse.X, mouse.Y)))
                {
                    done = false;
                    waitTime = 0;
                    currentStage = new CuttingScreen(type, Game1.screenRect, content.Load<Texture2D>("CookingStageTextures/CuttingStageTextures/dough"), content.Load<Texture2D>("CookingStageTextures/CuttingStageTextures/circle outline"), content.Load<Texture2D>("CookingStageTextures/circle dough"), content.Load<Texture2D>("whitePixel"));
                }
            }
            else if (done)
            {
                readyToMoveOn = true;
            }
            else if(waitTime > 0)
            {

            }
            else if(currentStage.isDone()) {
                if (!hasWaited)
                {
                    waitTime = 120;
                    if (done)
                        waitTime = 180;
                    hasWaited = true;
                }
                else if(!isTransitioning) 
                {
                    isTransitioning = true;
                    isFadingIn = true;
                    alpha.A = 0;
                }
                else if(isFadingIn) 
                {
                    alpha.A+=3;
                    if(alpha.A >= 253)
                    {
                        alpha.A = 255;
                        isFadingIn = false;
                        isFadingOut = true;
                    }
                    
                }
                else if(isFadingOut)
                {
                    alpha.A-=3;
                    if(alpha.A <= 2)
                    {
                        isFadingOut = false;
                        isTransitioning = false;
                        alpha.A = 0;
                        accuracy+=currentStage.getAccuracy();
                        if(currentStage.getStage() == CookStage.Cooking)
                        {
                            accuracy/=3.0;
                        }
                        switch(currentStage.getStage()) 
                        {
                            case CookStage.Cutting:
                                List<KeyValuePair<Rectangle, Topping>> l = new List<KeyValuePair<Rectangle, Topping>>();
                                l.Add(new KeyValuePair<Rectangle, Topping>(new Rectangle(700, 600, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), Topping.pepperoni));
                                Texture2D toppings = content.Load<Texture2D>("CookingStageTextures/ToppingsTextures/Toppings");
                                Color[] data = new Color[toppings.Width * toppings.Height];
                                toppings.GetData(data);
                                for(int i = 0; i < data.Length; i++) {
                                    if(!(data[i].A == 0 && data[i].R == 0 && data[i].G == 0 && data[i].B == 0)) {
                                        data[i] = Color.White;
                                    }
                                }
                                Texture2D whiteout = new Texture2D(gd, toppings.Width, toppings.Height);
                                whiteout.SetData(data);
                                currentStage = new ToppingScreen(gd, type, content.Load<Texture2D>("CookingStageTextures/ToppingsTextures/Bowls"), content.Load<Texture2D>("CookingStageTextures/ToppingsTextures/Toppings1"), whiteout, content.Load<Texture2D>("CookingStageTextures/circle dough"), l);
                                break;

                            case CookStage.Toppings:
                                currentStage = new OvenScreen(type, content.Load<Texture2D>("CookingStageTextures/OvenTextures/pizza"), content.Load<Texture2D>("CookingStageTextures/OvenTextures/Ovenbg"), content.Load<Texture2D>("CookingStageTextures/OvenTextures/place"), 10, content.Load<SpriteFont>("SpriteFont1"), content.Load<Texture2D>("CookingStageTextures/OvenTextures/amazing"));
                                drawTable = false;
                                break;
                        }
                        hasWaited = false;
                    }
                }
            }
        }
    }
}
