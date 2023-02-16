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

        public CookingManager(ContentManager content, Pizza type) : base(content) {
            this.type = type;
            // currentStage = new CuttingScreen()
        }

        public override void draw(SpriteBatch spriteBatch) {
            currentStage.draw(spriteBatch);
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
