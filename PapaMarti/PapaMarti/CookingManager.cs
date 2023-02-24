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
        private readonly ContentManager content;

        public CookingManager(ContentManager content, Pizza type) : base(content) {
            this.type = type;
            this.content = content;
            // currentStage = new CuttingScreen()
            currentStage = new ToppingScreen(type, content.Load<Texture2D>(@"CookingStageTextures\ToppingsTextures\bowl"), content.Load<Texture2D>(@"CookingStageTextures\ToppingsTextures\Toppings"));
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
                        break;

                    case CookStage.Toppings:
                        // currentStage = new CookingScreen
                        break;
                }
            }
        }
    }
}
