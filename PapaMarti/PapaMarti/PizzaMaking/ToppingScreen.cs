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
    public class indTopping//individual topping class because jacob screwed everything up and i just need 1 rectanglr
    {
        public Topping topping;
        public Rectangle source;
        Random rand;

        public indTopping(Topping topping)
        {
            this.topping = topping;
            rand = new Random();
            int xpos = rand.Next(3) * 8;
            source = new Rectangle(xpos, topping.yPos, 8, 8);
        }
    }

    public class ToppingScreen : CookingStage {
        private readonly Texture2D bowl;
        private readonly Texture2D toppings;
        private readonly Texture2D dough;
        private readonly Rectangle doughRect;
        private readonly List<Topping> toppingOrder;

        private indTopping currentClicked;
        private Rectangle toppingRect;
        private Queue<KeyValuePair<Rectangle, indTopping>> toppingPos;
        private Point prevMouse;
        private double passedTime;
        private Random rand;

        /// <summary>
        /// Creates a new topping screen to complete the topping stage of creating the pizza
        /// </summary>
        public ToppingScreen(Pizza type, Texture2D bowl, Texture2D toppings, Texture2D dough) : base(type) {
            this.bowl = bowl;
            this.toppings = toppings;
            this.dough = dough;
            double size = Game1.screenRect.Height * 0.64;
            doughRect = new Rectangle((int) (Game1.screenRect.Width - size) / 2, (int) ((Game1.screenRect.Height - size) / 2), (int) size, (int) size);
            passedTime = 0;
            currentClicked = null;
            toppingRect = new Rectangle();
            toppingPos = new Queue<KeyValuePair<Rectangle, indTopping>>();
            prevMouse = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            rand = new Random();
        }

        override
        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(dough, doughRect, Color.White);
            foreach(ToppingContainer t in ToppingContainer.containers) {
                t.draw(spriteBatch, bowl, toppings);
            }

            foreach(KeyValuePair<Rectangle, indTopping> k in toppingPos) {
                
                spriteBatch.Draw(toppings, k.Key, k.Value.source, Color.White);
            }

            if (currentClicked != null)
            {
                spriteBatch.Draw(toppings, toppingRect, currentClicked.source, Color.White);
            }
        }

        override
        public void update(GameTime time) {
            MouseState m = Mouse.GetState();
            passedTime += time.ElapsedGameTime.TotalSeconds;
            if(m.LeftButton == ButtonState.Pressed) {
                if(currentClicked == null) {
                    foreach(ToppingContainer c in ToppingContainer.containers) {
                        if(c.location.Contains(new Point(m.X, m.Y))) {
                            currentClicked = new indTopping(c.type);
                        }
                    }
                }

                else {
                    toppingRect = new Rectangle(m.X - (ToppingContainer.TOPPING_SIZE / 2), m.Y - (ToppingContainer.TOPPING_SIZE / 2), ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE);
                    if(Math.Pow(m.X - 930, 2) + Math.Pow(m.Y - 510, 2) < 75625 && Math.Sqrt(Math.Pow(m.X - prevMouse.X, 2) + Math.Pow(m.Y - prevMouse.Y, 2)) > 32 && !currentClicked.topping.isDragAndDrop) {
                        if(toppingPos.Count > 500)
                            toppingPos.Dequeue();
                        toppingPos.Enqueue(new KeyValuePair<Rectangle, indTopping>(new Rectangle(m.X, m.Y, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), currentClicked));
                        prevMouse.X = m.X;
                        prevMouse.Y = m.Y;
                    }
                }
            }

            else {
                if(currentClicked != null && Math.Pow(m.X - 930, 2) + Math.Pow(m.Y - 540, 2) < 75625 && currentClicked.topping.isDragAndDrop) {
                    toppingPos.Enqueue(new KeyValuePair<Rectangle, indTopping>(new Rectangle(m.X, m.Y, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), currentClicked));
                }
                currentClicked = null;
            }
        }

        override
        public bool isDone() {
            return passedTime > 10;
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
