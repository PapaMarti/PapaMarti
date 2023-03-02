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
        private readonly Texture2D dough;
        private readonly Texture2D table;
        private readonly Rectangle doughRect;
        private readonly List<Topping> toppingOrder;

        private Topping currentClicked;
        private Rectangle toppingRect;
        private Dictionary<Rectangle, Topping> toppingPos;

        /// <summary>
        /// Creates a new topping screen to complete the topping stage of creating the pizza
        /// </summary>
        public ToppingScreen(Pizza type, Texture2D bowl, Texture2D toppings, Texture2D dough) : base(type) {
            this.bowl = bowl;
            this.toppings = toppings;
            this.dough = dough;
            double size = Game1.screenRect.Height * 0.64;
            doughRect = new Rectangle((int) (Game1.screenRect.Width - size) / 2, (int) ((Game1.screenRect.Height - size) / 2), (int) size, (int) size);
            currentClicked = null;
            toppingRect = new Rectangle();
            toppingPos = new Dictionary<Rectangle, Topping>();
        }

        override
        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(dough, doughRect, Color.White);
            foreach(ToppingContainer t in ToppingContainer.containers) {
                t.draw(spriteBatch, bowl, toppings);
            }

            if(currentClicked != null)
                spriteBatch.Draw(toppings, toppingRect, currentClicked.textureRect, Color.White);
        }

        override
        public void update(GameTime time) {
            MouseState m = Mouse.GetState();
            if(m.LeftButton == ButtonState.Pressed) {
                if(currentClicked == null) {
                    foreach(ToppingContainer c in ToppingContainer.containers) {
                        if(c.location.Contains(new Point(m.X, m.Y))) {
                            currentClicked = c.type;
                        }
                    }
                }

                else {
                    toppingRect = new Rectangle(m.X - (ToppingContainer.TOPPING_SIZE / 2), m.Y - (ToppingContainer.TOPPING_SIZE / 2), ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE);
                }

                if(Math.Pow(m.X - 960, 2) + Math.Pow(m.Y - 540, 2) < 345.6) {
                    toppingPos.Add(new Rectangle(m.X, m.Y, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), currentClicked);
                }
            }

            else {
                currentClicked = null;
            }
        }

        override
        public bool isDone() {
            return true;
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
