using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PapaMarti {

    public class Topping {
        public static readonly Topping cheese;
        public static readonly Topping pepperoni;
        public static readonly Topping mushroom;
        public static readonly Topping vegetable;
        public static readonly Topping spicyThing;
        public static readonly Topping meat;
        public static readonly Topping blueThing;
        public static readonly Topping sauce;
        public static readonly List<Topping> toppings;

        static Topping() {
            cheese = new Topping(new Rectangle(0, 16, 16, 16), false);
            pepperoni = new Topping(new Rectangle(0, 0, 16, 16), true);
            mushroom = new Topping(new Rectangle(16, 0, 16, 16), true);
            vegetable = new Topping(new Rectangle(16, 16, 16, 16), true);
            spicyThing = new Topping(new Rectangle(32, 0, 16, 16), true);
            meat = new Topping(new Rectangle(32, 16, 16, 16), true);
            blueThing = new Topping(new Rectangle(48, 0, 16, 16), true);
            sauce = new Topping(new Rectangle(48, 16, 16, 16), false);
            toppings = new List<Topping>();
            toppings.Add(cheese);
            toppings.Add(pepperoni);
            toppings.Add(mushroom);
            toppings.Add(vegetable);
            toppings.Add(spicyThing);
            toppings.Add(meat);
            toppings.Add(blueThing);
            toppings.Add(sauce);
        }

        public readonly Rectangle textureRect;
        public readonly bool isDragAndDrop;

        private Topping(Rectangle rect, bool isDragAndDrop) {
            this.textureRect = rect;
            this.isDragAndDrop = isDragAndDrop;
        }
    }
    public class Pizza {
        public readonly List<Rectangle> outline;
        public readonly List<Topping> toppings;
        int cookTime;
        int quality;

        /// <summary>
        /// Creates a pizza that holds its specific values.
        /// </summary>
        /// <param name="cookTime">The amount of time it takes to cook this pizza.</param>
        /// <param name="toppings">The types of toppings that this pizza will have.</param>
        /// <param name="outline">List of Rectangles outlining where the pizza is supposed to be cut</param>
        public Pizza(List<Rectangle> outline, List<Topping> toppings, int cookTime) {
            this.outline = outline;
            this.toppings = toppings;
            this.cookTime = cookTime;
            this.quality = 0;
        }


    }
}
