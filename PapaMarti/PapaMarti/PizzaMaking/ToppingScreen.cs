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
        private static Color whiteoutColor = new Color(255, 255, 255, 100);
        private static Color clear = new Color(0, 0, 0, 0);
        private static Color brown = new Color(57, 38, 17, 255);
        private static Color brown2 = new Color(42, 28, 13, 255);
        private static Color brown3 = new Color(34, 16, 6, 255);

        private readonly Texture2D bowl;
        private readonly Texture2D toppings;
        private readonly Texture2D whiteout;
        private readonly GraphicsDevice gd;
        private Texture2D dough;
        private readonly Rectangle doughRect;
        private readonly List<KeyValuePair<Rectangle, Topping>> actualToppingPos;

        private Topping currentClicked;
        private Rectangle toppingRect;
        private Queue<KeyValuePair<Rectangle, Topping>> toppingPos;
        private Point prevMouse;
        private double passedTime;

        /// <summary>
        /// Creates a new topping screen to complete the topping stage of creating the pizza
        /// </summary>
        public ToppingScreen(GraphicsDevice gd, Pizza type, Texture2D bowl, Texture2D toppings, Texture2D whiteout, Texture2D dough, List<KeyValuePair<Rectangle, Topping>> actualToppingPos) : base(type) {
            this.gd = gd;
            this.whiteout = whiteout;
            this.bowl = bowl;
            this.toppings = toppings;
            this.dough = dough;
            this.actualToppingPos = actualToppingPos;
            double size = Game1.screenRect.Height * 0.64;
            doughRect = new Rectangle((int) (Game1.screenRect.Width - size) / 2, (int) ((Game1.screenRect.Height - size) / 2), (int) size, (int) size);
            passedTime = 0;
            currentClicked = null;
            toppingRect = new Rectangle();
            toppingPos = new Queue<KeyValuePair<Rectangle, Topping>>();
            prevMouse = new Point(Mouse.GetState().X, Mouse.GetState().Y);
        }

        override
        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(dough, doughRect, Color.White);
            foreach(ToppingContainer t in ToppingContainer.containers) {
                t.draw(spriteBatch, bowl, toppings);
            }

            foreach(KeyValuePair<Rectangle, Topping> k in toppingPos) {
                if(k.Value == Topping.cheese || k.Value == Topping.sauce)
                    spriteBatch.Draw(toppings, k.Key, k.Value.textureRect, Color.White);
            }

            foreach(KeyValuePair<Rectangle, Topping> t in actualToppingPos) {
                spriteBatch.Draw(whiteout, t.Key, t.Value.textureRect, whiteoutColor);
            }

            foreach(KeyValuePair<Rectangle, Topping> k in toppingPos) {
                if(k.Value != Topping.cheese && k.Value != Topping.sauce)
                    spriteBatch.Draw(toppings, k.Key, k.Value.textureRect, Color.White);
            }

            if(currentClicked != null)
                spriteBatch.Draw(toppings, toppingRect, currentClicked.textureRect, Color.White);
        }

        override
        public void update(GameTime time) {
            MouseState m = Mouse.GetState();
            passedTime += time.ElapsedGameTime.TotalSeconds;
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
                    if(Math.Pow(m.X - 930, 2) + Math.Pow(m.Y - 510, 2) < 75625 && Math.Sqrt(Math.Pow(m.X - prevMouse.X, 2) + Math.Pow(m.Y - prevMouse.Y, 2)) > 32 && !currentClicked.isDragAndDrop) {
                        if(toppingPos.Count > 500)
                            toppingPos.Dequeue();
                        toppingPos.Enqueue(new KeyValuePair<Rectangle, Topping>(new Rectangle(m.X, m.Y, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), currentClicked));
                        prevMouse.X = m.X;
                        prevMouse.Y = m.Y;
                    }
                }
            }

            else {
                if(currentClicked != null && Math.Pow(m.X - 930, 2) + Math.Pow(m.Y - 540, 2) < 75625 && currentClicked.isDragAndDrop) {
                    toppingPos.Enqueue(new KeyValuePair<Rectangle, Topping>(new Rectangle(m.X, m.Y, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), currentClicked));
                }
                currentClicked = null;
            }
        }

        override
        public bool isDone() {
            int t = 0;
            foreach(KeyValuePair<Rectangle, Topping> top in toppingPos)
                if(top.Value != Topping.cheese && top.Value != Topping.sauce)
                    t++;
            bool done = t == actualToppingPos.Count();

            if(done) {
                Color[] data = new Color[Game1.screenRect.Width * Game1.screenRect.Height];
                Color[] cropped = new Color[550 * 550];
                for(int i = 567055; i < 1623605; i++) {
                    cropped[i - 567055] = data[i];
                    if(cropped[i - 567055].Equals(brown) || cropped[i - 567055].Equals(brown2) || cropped[i - 567055].Equals(brown3)) {
                        cropped[i - 567055] = clear;
                    }
                }

                dough = new Texture2D(gd, 550, 550);
                dough.SetData(cropped);
            }

            return done;
        }

        override
        public double getAccuracy() {
            double t = 0;
            foreach(KeyValuePair<Rectangle, Topping> top in toppingPos)
                if(top.Value != Topping.cheese && top.Value != Topping.sauce)
                    t++;

            return t / actualToppingPos.Count;
        }

        override
        public CookStage getStage() {
            return CookStage.Toppings;
        }

        public override Texture2D getModifiedPizza() {
            return dough;
        }
    }
}
