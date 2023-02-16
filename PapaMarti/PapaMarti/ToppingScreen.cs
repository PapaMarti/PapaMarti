using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PapaMarti
{
    public class ToppingScreen : CookingStage {
        private static Rectangle[] binRects = new Rectangle[] { new Rectangle(100, 100, 20, 20), new Rectangle(100, 150, 20, 20), 
            new Rectangle(100, 200, 20, 20), new Rectangle(300, 100, 20, 20), new Rectangle(300, 150, 20, 20), new Rectangle(300, 200, 20, 20)};
        Texture2D background;
        Texture2D dough;
        Texture2D bins;
        Rectangle screenSize;

        public ToppingScreen(Rectangle screenSize, Texture2D background, Texture2D dough, Texture2D bins, Pizza pizza) : base(pizza) {
            this.screenSize = screenSize;
            this.background = background;
            this.dough = dough;
            this.bins = bins;
        }

        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(background, screenSize, Color.White);
            foreach(Rectangle r in binRects) {
                spriteBatch.Draw(bins, r, Color.Gray);
            }
        }

        public override double getAccuracy() {
            return 0;
        }

        public override CookStage getStage() {
            return CookStage.Toppings;
        }

        public override bool isDone() {
            return false;
        }

        public override void update(GameTime time) {
        }
    }
}
