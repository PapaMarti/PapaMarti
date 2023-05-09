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

    public enum CookingManagerLevel
    {
        Tutorial,
        Mainquest,
        Sidequest
    }

    public class CookingManager : StageManager {
        private readonly Pizza type;
        public CookingStage currentStage;
        private Texture2D baseRect;
        private bool isTransitioning;
        private bool isFadingIn, isFadingOut;
        private Color alpha;
        public double accuracy; //a number from 0.0 to 1.0
        private int waitTime; //in 1/60th of a seconds
        private bool hasWaited;
        private readonly CookingManagerLevel level;
        SpriteFont font;
        bool drawTable;
        Texture2D table;
        Rectangle retryButton;
        bool done;
        bool readyToMoveOn;
        bool isTutorial;
        Texture2D pixel;
        Texture2D backgroundYes;
        Texture2D white;
        Texture2D whiteout;
        Texture2D toppings;
        Rectangle accuracyBanner;
        ContentManager content;

        List<TextCard> textCards;

        public CookingManager(Pizza type, MapLocation location, bool isTutorial, CookingManagerLevel level) : base(location) {
            this.level = level;
            this.type = type;
            isTransitioning = false;
            isFadingIn = false;
            this.isTutorial = isTutorial;
            isFadingOut = false;
            int retryWidth = Game1.screenRect.Width / 10;
            int retryHeight = Game1.screenRect.Height / 12;
            retryButton = new Rectangle((Game1.screenRect.Width - retryWidth) / 2, (Game1.screenRect.Height - retryHeight) / 2 + 300, retryWidth, retryHeight);
            alpha = new Color(0, 0, 0, 0);
            accuracy = 0.0;
            waitTime = 0;
            hasWaited = false;
            drawTable = true;
            readyToMoveOn = false;
            int accHeight = 70;
            accuracyBanner = new Rectangle(0, (Game1.screenRect.Height - accHeight) / 2, Game1.screenRect.Width, accHeight);
            this.textCards = new List<TextCard>();
        }

        public override void contentify(ContentManager content, Player p)
        {
            table = content.Load<Texture2D>("CookingStageTextures/Table");
            font = content.Load<SpriteFont>("text01");
            pixel = content.Load<Texture2D>("whitePixel");
            backgroundYes = content.Load<Texture2D>("CookingStageTextures/OvenTextures/Ovenbg");
            if (isTutorial)
            {
                string introduction = "Welcome to Papa Marti's pizza shop! To make a pizza for the customers, first we must cut out the dough of the pizza.";
                textCards.Add(new TextCard(content, introduction, String.Empty));
            }
            string cuttingInstruction = "Use the mouse to trace the outline shown on the dough. Make sure you're careful, though, because your accuracy will be kept track of, and you might even have to redo it!";
            textCards.Add(new TextCard(content, cuttingInstruction, String.Empty));

            white = content.Load<Texture2D>("whitePixel");
            this.content = content;
            toppings = content.Load<Texture2D>("CookingStageTextures/ToppingsTextures/Toppings1");
            whiteout = new Texture2D(toppings.GraphicsDevice, toppings.Width, toppings.Height);
            Color[] data = new Color[whiteout.Width * whiteout.Height];
            toppings.GetData(data);
            for (int i = 0; i < data.Length; i++)
            {
                if (!(data[i].A == 0 && data[i].R == 0 && data[i].G == 0 && data[i].B == 0))
                {
                    data[i].A = 125;
                    //data[i].R = (byte) (data[i].R * 1.5);
                    //data[i].G = (byte) (data[i].G * 1.5);
                    //data[i].B = (byte) (data[i].B * 1.5);
                }
            }
            whiteout.SetData(data);
            baseRect = content.Load<Texture2D>("whitePixel");

            if (type.shape == PizzaShape.Circle)
                currentStage = new CuttingScreen(type, Game1.screenRect, content.Load<Texture2D>("CookingStageTextures/CuttingStageTextures/dough"), content.Load<Texture2D>("CookingStageTextures/CuttingStageTextures/circle outline"), content.Load<Texture2D>("CookingStageTextures/circle dough"), baseRect);
        }

        public override void draw(SpriteBatch spriteBatch) {
            if (drawTable)
                spriteBatch.Draw(table, Game1.screenRect, Color.White);
            else
                spriteBatch.Draw(backgroundYes, Game1.screenRect, new Rectangle(0,0,240,135), Color.White);

            currentStage.draw(spriteBatch);

            if (textCards.Count > 0)
                textCards[0].draw(spriteBatch);

            if (isTransitioning) {
                spriteBatch.Draw(baseRect, Game1.screenRect, alpha);
            }
            if(waitTime > 0 && !done)
            {
                string accuracyText = "Accuracy: " + Math.Round(currentStage.getAccuracy() * 100) + "%";
                spriteBatch.Draw(white, accuracyBanner, Color.Gray);
                spriteBatch.DrawString(font, accuracyText, new Vector2((Game1.screenRect.Width - font.MeasureString(accuracyText).X) / 2, (Game1.screenRect.Height - font.MeasureString(accuracyText).Y) / 2), Color.Black);
            }
            if(done && !readyToMoveOn)
            {
                string accuracyText = "Total Accuracy: " + Math.Round(accuracy * 100) + "%";
                spriteBatch.Draw(white, accuracyBanner, Color.Gray);
                spriteBatch.DrawString(font, accuracyText, new Vector2((Game1.screenRect.Width - font.MeasureString(accuracyText).X) / 2, (accuracyBanner.Height - font.MeasureString(accuracyText).Y) / 2 + accuracyBanner.Y), Color.Black);
                spriteBatch.Draw(pixel, retryButton, Color.LightBlue);
                string retry = "Retry?";
                spriteBatch.DrawString(font, retry, new Vector2((retryButton.Width - font.MeasureString(retry).X) / 2 + retryButton.X, (retryButton.Height - font.MeasureString(retry).Y) / 2 + retryButton.Y), Color.Black);
                string timeLeft = waitTime / 60 + "";
                spriteBatch.DrawString(font, timeLeft, new Vector2((Game1.screenRect.Width - font.MeasureString(timeLeft).X) / 2, Game1.screenRect.Height - font.MeasureString(timeLeft).Y - 20), Color.White);
            }
        }

        public override GameStage getStage() {
            return GameStage.Cooking;
        }

        public override bool isDone() {
            if (readyToMoveOn && level != CookingManagerLevel.Sidequest) QuestTracker.advanceMainquest();
            return readyToMoveOn;
        }

        public override void update(GameTime time) {
            currentStage.update(time);

            if(textCards.Count > 0)
            {
                textCards[0].update();

                if (textCards[0].isDone())
                    textCards.RemoveAt(0);
            }

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
                    drawTable = true;
                    waitTime = 0;
                    accuracy = 0.0;
                    accuracyBanner.Y += 300;
                    currentStage = new CuttingScreen(type, Game1.screenRect, content.Load<Texture2D>("CookingStageTextures/CuttingStageTextures/dough"), content.Load<Texture2D>("CookingStageTextures/CuttingStageTextures/circle outline"), content.Load<Texture2D>("CookingStageTextures/circle dough"), content.Load<Texture2D>("whitePixel"));
                }
            }
            else if(done && !hasWaited)
            {
                accuracyBanner.Y -= 300;
                waitTime = 360;
                hasWaited = true;
                accuracy += currentStage.getAccuracy();
                if (currentStage.getStage() == CookStage.Cooking)
                {
                    accuracy /= 3.0;
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

                        accuracy += currentStage.getAccuracy();
                        switch (currentStage.getStage())
                        {
                            case CookStage.Cutting:
                                string toppingsInstruction = "Now you need to add the toppings! First drag your mouse from the sauce and cheese bins across the pizza to spead them, and then add pepperoni by dragging and dropping them onto the pizza from their bowl.";
                                textCards.Add(new TextCard(content, toppingsInstruction, String.Empty));

                                List<KeyValuePair<Rectangle, Topping>> l = new List<KeyValuePair<Rectangle, Topping>>();
                                l.Add(new KeyValuePair<Rectangle, Topping>(new Rectangle(700, 600, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), Topping.pepperoni));

                                
                                currentStage = new ToppingScreen(type, content.Load<Texture2D>("CookingStageTextures/ToppingsTextures/Bowls"), content.Load<Texture2D>("CookingStageTextures/ToppingsTextures/Toppings1"), whiteout, content.Load<Texture2D>("CookingStageTextures/circle dough"), l);
                                break;

                            case CookStage.Toppings:
                                string cookingInstructions = "Now it's time to cook the pizza! Use the mouse to drag and drop the pizza into the oven to begin the cooking process, and drag it back to remove it. Cook the pizza for " + type.cookTime + " seconds, but be careful, you won't have a timer!";
                                textCards.Add(new TextCard(content, cookingInstructions, String.Empty));
                                string moreInstructions = "You can put the pizza back into the oven again after it has already been in, but only once! Your current accuracy after each attempt will be shown on the bottom right of the screen. Press the enter key when you are satisfied with your pizza. ";
                                textCards.Add(new TextCard(content, moreInstructions, String.Empty));

                                currentStage = new OvenScreen(content, type, content.Load<Texture2D>("CookingStageTextures/OvenTextures/pizza"), content.Load<Texture2D>("CookingStageTextures/OvenTextures/Ovenbg"), content.Load<Texture2D>("CookingStageTextures/OvenTextures/place"), content.Load<SpriteFont>("SpriteFont1"), content.Load<Texture2D>("CookingStageTextures/OvenTextures/amazing"));
                                drawTable = false;
                                break;
                        }
                    }
                    
                }
            }
            else if (isFadingOut)
            {
                alpha.A -= 3;
                if (alpha.A <= 2)
                {
                    isFadingOut = false;
                    isTransitioning = false;
                    alpha.A = 0;
                    hasWaited = false;
                }
            }
        }

        
    }
}
