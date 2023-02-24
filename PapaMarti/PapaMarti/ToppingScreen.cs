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
    public class ToppingScreen : CookingStage {
        private readonly Texture2D bowl;
        private readonly Texture2D toppings;

        /// <summary>
        /// Creates a new topping screen to complete the topping stage of creating the pizza
        /// </summary>
        public ToppingScreen(Pizza type, Texture2D bowl, Texture2D toppings) : base(type) {
            this.bowl = bowl;
            this.toppings = toppings;
        }

        override
        public void draw(SpriteBatch spriteBatch) {
            foreach(ToppingContainer t in ToppingContainer.containers) {
                t.draw(spriteBatch, bowl, toppings);
            }
        }

        override
        public void update(GameTime time) {

        }

        override
        public bool isDone() {
            return false;
        }

        override
        public double getAccuracy() {
            return 0;
        }

        override
        public CookStage getStage() {
            return CookStage.Toppings;
        }
    }
}
