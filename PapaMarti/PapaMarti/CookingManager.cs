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

        public CookingManager(GraphicsDevice gd, ContentManager content, Pizza type) : base(content) {
            this.type = type;
            this.content = content;
            // currentStage = new CuttingScreen()
            Texture2D baseRect = new Texture2D(gd, 1, 1);
            baseRect.SetData(new Color[] { Color.White });
            currentStage = new ToppingScreen(new Rectangle(0, 0, 1920, 1080),
                            content.Load<Texture2D>("shop-layout"), baseRect, baseRect, type);
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
                        currentStage = new ToppingScreen(new Rectangle(0, 0, 1920, 1080), 
                            content.Load<Texture2D>("shop-layout"), null, null, type);
                        break;

                    case CookStage.Toppings:
                        // currentStage = new CookingScreen
                        break;
                }
            }
        }
    }
}
