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
        private static Color whiteoutColor = new Color(255, 255, 255, 100);
        private static Color clear = new Color(0, 0, 0, 0);
        private static Color brown = new Color(57, 38, 17, 255);
        private static Color brown2 = new Color(42, 28, 13, 255);
        private static Color brown3 = new Color(34, 16, 6, 255);

        private readonly Texture2D bowl;
        private readonly Texture2D toppings;
        private readonly Texture2D whiteout;
        private readonly GraphicsDevice gd;
        private readonly Texture2D dough;
        private readonly Rectangle doughRect;
        private readonly List<KeyValuePair<Rectangle, Topping>> actualToppingPos;
        private List<ToppingList> toppingLists;

        private indTopping currentClicked;
        private Rectangle toppingRect;
        private Queue<KeyValuePair<Rectangle, indTopping>> toppingPos;
        private Point prevMouse;
        private double passedTime;
        private Random rand;

        /// <summary>
        /// Creates a new topping screen to complete the topping stage of creating the pizza
        /// </summary>
        public ToppingScreen(GraphicsDevice gd, Pizza type, Texture2D bowl, Texture2D toppings, Texture2D whiteout, Texture2D dough, List<KeyValuePair<Rectangle, Topping>> actualToppingPos) : base(type) {
            this.gd = gd;
            this.whiteout = whiteout;
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


            this.actualToppingPos = actualToppingPos;
            this.toppingLists = new List<ToppingList>();
            foreach(Topping t in Topping.toppings) {
                if(t == Topping.cheese || t == Topping.sauce)
                    continue;
                int numtops = 0;
                foreach(KeyValuePair<Rectangle, Topping> k in actualToppingPos) {
                    if(k.Value == t)
                        numtops++;
                }

                toppingLists.Add(new ToppingList(t, numtops));
            }
        }

        override
        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(dough, doughRect, Color.White);

            foreach(KeyValuePair<Rectangle, Topping> kv in actualToppingPos) {
                spriteBatch.Draw(whiteout, kv.Key, kv.Value.spritesheetRect, Color.White);
            }

            foreach(ToppingContainer t in ToppingContainer.containers) {
                t.draw(spriteBatch, bowl, toppings);
            }

            foreach(KeyValuePair<Rectangle, indTopping> k in toppingPos) {
                if(k.Value.topping == Topping.cheese || k.Value.topping == Topping.sauce)
                    spriteBatch.Draw(toppings, k.Key, k.Value.source, Color.White);
            }

            foreach(KeyValuePair<Rectangle, indTopping> k in toppingPos) {
                if(k.Value.topping != Topping.cheese && k.Value.topping != Topping.sauce)
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
                        if(toppingPos.Count > 500) {
                            decTopList(toppingPos.Peek().Value.topping);
                            toppingPos.Dequeue();
                        }
                        toppingPos.Enqueue(new KeyValuePair<Rectangle, indTopping>(new Rectangle(m.X, m.Y, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), currentClicked));
                        incTopList(currentClicked.topping);
                        prevMouse.X = m.X;
                        prevMouse.Y = m.Y;
                    }
                }
            }

            else {
                if(currentClicked != null && Math.Pow(m.X - 930, 2) + Math.Pow(m.Y - 540, 2) < 75625 && currentClicked.topping.isDragAndDrop) {
                    toppingPos.Enqueue(new KeyValuePair<Rectangle, indTopping>(new Rectangle(m.X, m.Y, ToppingContainer.TOPPING_SIZE, ToppingContainer.TOPPING_SIZE), currentClicked));
                    incTopList(currentClicked.topping);
                }
                currentClicked = null;
            }
        }

        override
        public bool isDone() {
            bool done = true;
            bool hasSauce = false;
            bool hasCheese = false;

            foreach(KeyValuePair<Rectangle, indTopping> kv in toppingPos) {
                hasSauce = hasSauce || kv.Value.topping == Topping.sauce;
            }

            foreach(KeyValuePair<Rectangle, indTopping> kv in toppingPos)
                hasCheese = hasCheese || kv.Value.topping == Topping.cheese;

            foreach(ToppingList l in toppingLists)
                done = done && l.hasAll();

            return hasCheese && hasSauce && done;
        }

        override
        public double getAccuracy() {
            return 1;
        }

        override
        public CookStage getStage() {
            return CookStage.Toppings;
        }

        private void incTopList(Topping type) {
            foreach(ToppingList l in toppingLists)
                if(l.type == type)
                    l.has++;
        }

        private void decTopList(Topping type) {
            foreach(ToppingList l in toppingLists)
                if(l.type == type)
                    l.has--;
        }
    }
}
