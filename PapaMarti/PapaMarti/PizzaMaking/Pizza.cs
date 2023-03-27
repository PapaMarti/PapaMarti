using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace PapaMarti {

    public class Topping {
        public static readonly Topping cheese;
        public static readonly Topping pepperoni;
        public static readonly Topping mushroom;
        public static readonly Topping capsicum;
        public static readonly Topping jalapeno;
        public static readonly Topping meat;
        public static readonly Topping onion;
        public static readonly Topping sauce;
        public static readonly List<Topping> toppings;

        static Topping() {
            cheese = new Topping(56, new Rectangle(96, 32, 32, 32), false, new Rectangle(0, 56, 8, 8));
            pepperoni = new Topping(48, new Rectangle(64, 32, 32, 32), true, new Rectangle(0, 48, 8, 8));
            mushroom = new Topping(40, new Rectangle(32, 32, 32, 32), true, new Rectangle(0, 40, 8, 8));
            capsicum = new Topping(32, new Rectangle(0, 32, 32, 32), true, new Rectangle(0, 32, 8, 8));
            jalapeno = new Topping(24, new Rectangle(96, 0, 32, 32), true, new Rectangle(0, 24, 8, 8));
            meat = new Topping(16, new Rectangle(64, 0, 32, 32), true, new Rectangle(0, 16, 8, 8));
            onion = new Topping(8, new Rectangle(32, 0, 32, 32), true, new Rectangle(0, 8, 8, 8));
            sauce = new Topping(0, new Rectangle(0, 0, 32, 32), false, new Rectangle(0, 0, 8, 8));
            toppings = new List<Topping>();
            toppings.Add(cheese);
            toppings.Add(pepperoni);
            toppings.Add(mushroom);
            toppings.Add(capsicum);
            toppings.Add(jalapeno);
            toppings.Add(meat);
            toppings.Add(onion);
            toppings.Add(sauce);
        }

        public readonly int yPos;//y position inside the texture itself
        public readonly Rectangle bowlRect;
        public readonly bool isDragAndDrop;
        public readonly Rectangle spritesheetRect;
        public int type;//which one of the 3 textures is this

        private Topping(int yPos, Rectangle bowlRect, bool isDragAndDrop, Rectangle spritesheetRect) {
            this.yPos = yPos;
            this.bowlRect = bowlRect;
            this.isDragAndDrop = isDragAndDrop;
            this.spritesheetRect = spritesheetRect;
        }
    }
    public class Pizza {
        public readonly List<Rectangle> outline;
        public readonly List<Topping> toppings;
        public readonly PizzaShape shape;
        int cookTime;
        int quality;

        /// <summary>
        /// Creates a pizza that holds its specific values.
        /// </summary>
        /// <param name="cookTime">The amount of time it takes to cook this pizza.</param>
        /// <param name="toppings">The types of toppings that this pizza will have.</param>
        /// <param name="outline">List of Rectangles outlining where the pizza is supposed to be cut</param>
        public Pizza(PizzaShape shape, List<Rectangle> outline, List<Topping> toppings, int cookTime) {
            this.outline = outline;
            this.toppings = toppings;
            this.cookTime = cookTime;
            this.shape = shape;
            this.quality = 0;
        }


    }
}
