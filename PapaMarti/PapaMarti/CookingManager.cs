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
        private Rectangle screenRect;
        private Color alpha;
        private double accuracy;

        public CookingManager(ContentManager content, Rectangle screenRect, Texture2D baseRect, Pizza type) : base(content) {
            this.type = type;
            isTransitioning = false;
            this.baseRect = baseRect;
            alpha = new Color(255, 255, 255, 0);
            this.screenRect = screenRect;
            accuracy = 0.0;
            if(type.shape == PizzaShape.Circle)
                currentStage = new CuttingScreen(type, this.screenRect, content.Load<Texture2D>("dough"), content.Load<Texture2D>("circle outline"), content.Load<Texture2D>("circle dough"), content.Load<Texture2D>("whitePixel"));
        }

        public override void draw(SpriteBatch spriteBatch) {
            currentStage.draw(spriteBatch);
            if(isTransitioning) {
                spriteBatch.Draw(baseRect, screenRect, alpha);
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
            if(currentStage.isDone()) {
                if(!isTransitioning) {
                    isTransitioning = true;
                    alpha.A = 0;
                }

                else if(alpha.A >= 255) {
                    isTransitioning = false;
                    alpha.A = 0;

                    switch(currentStage.getStage()) {
                        case CookStage.Cutting:
                            // currentStage = new ToppingsScreen
                            break;

                        case CookStage.Toppings:
                            // currentStage = new CookingScreen
                            break;
                    }
                }

                
            }
        }
    }
}
