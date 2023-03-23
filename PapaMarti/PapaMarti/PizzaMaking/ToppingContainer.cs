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
    public class ToppingContainer {
        public static int TOPPING_SIZE = 64;
        public static readonly List<ToppingContainer> containers;

        static ToppingContainer() {
            containers = new List<ToppingContainer>();
            int ypos = (Game1.screenRect.Height - 1000) / 2;
            int xpos = 75;
            foreach(Topping t in Topping.toppings) {
                containers.Add(new ToppingContainer(t, new Rectangle(xpos, ypos, 240, 240)));
                ypos += 240;

                if(ypos > 800) {
                    xpos = Game1.screenRect.Width - 75 - 240;
                    ypos = (Game1.screenRect.Height - 1000) / 2;
                }
            }
        }


        private readonly Rectangle toppingLocation;
        public readonly Rectangle location;
        public readonly Topping type;

        private ToppingContainer(Topping type, Rectangle location) {
            this.type = type;
            this.location = location;
            toppingLocation = new Rectangle(location.X + ((location.Width - TOPPING_SIZE) / 2), location.Y + ((location.Height - TOPPING_SIZE) / 2), TOPPING_SIZE, TOPPING_SIZE);
        }

        public void draw(SpriteBatch spritebatch, Texture2D sheet, Texture2D toppings) {
            spritebatch.Draw(sheet, location, type.bowlRect, Color.White);
            //spritebatch.Draw(toppings, toppingLocation, type.textureRect, Color.White);
        }
    }
}
