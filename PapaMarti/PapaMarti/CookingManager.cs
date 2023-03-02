using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti {

    public class CookingManager : StageManager {
        private readonly Pizza type;
        private CookingStage currentStage;
        private Texture2D baseRect;
        private bool isTransitioning;
        private bool isFadingIn, isFadingOut;
        private Color alpha;
        public double accuracy; //a number from 0.0 to 1.0
        private int waitTime; //in 1/60th of a seconds
        private bool hasWaited;
        SpriteFont font;

        public CookingManager(ContentManager content, Texture2D baseRect, Pizza type) : base(content) {
            this.type = type;
            isTransitioning = false;
            isFadingIn = false;
            isFadingOut = false;
            this.baseRect = baseRect;
            alpha = new Color(0, 0, 0, 0);
            accuracy = 0.0;
            waitTime = 0;
            hasWaited = false;
            font = content.Load<SpriteFont>("text01");
            if(type.shape == PizzaShape.Circle)
                currentStage = new CuttingScreen(type, Game1.screenRect, content.Load<Texture2D>("dough"), content.Load<Texture2D>("circle outline"), content.Load<Texture2D>("circle dough"), content.Load<Texture2D>("whitePixel"));
        }

        public override void draw(SpriteBatch spriteBatch) {
            currentStage.draw(spriteBatch);
            if(isTransitioning) {
                spriteBatch.Draw(baseRect, Game1.screenRect, alpha);
            }
            if(waitTime > 0)
            {
                string accuracyText = "Accuracy: " + Math.Round(currentStage.getAccuracy() * 100) + "%";
                spriteBatch.DrawString(font, accuracyText, new Vector2((screenRect.Width - font.MeasureString(accuracyText).X) / 2, (screenRect.Height - font.MeasureString(accuracyText).Y) / 2), Color.Black);
            }
        }

        public override GameStage getStage() {
            return GameStage.Cooking;
        }

        public override bool isDone() {
            return currentStage.getStage() == CookStage.Cooking && currentStage.isDone();
        }

        public override void update(GameTime time) {
            currentStage.update(time);
            waitTime--;
            if(waitTime > 0)
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
                                // currentStage = new ToppingsScreen
                                break;

                            case CookStage.Toppings:
                                // currentStage = new CookingScreen
                                break;
                        }
                        hasWaited = false;
                    }
                }
            }
        }
    }
}
