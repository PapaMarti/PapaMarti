using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PapaMarti {

    public class Topping {
        public static readonly Topping cheese;
        public static readonly Topping pepperoni;
        public static readonly Topping mushroom;
        public static readonly Topping vegetable;
        public static readonly List<Topping> toppings;

        static Topping() {
            cheese = new Topping(new Rectangle(0, 16, 16, 16));
            pepperoni = new Topping(new Rectangle(0, 0, 16, 16));
            mushroom = new Topping(new Rectangle(16, 0, 16, 16));
            vegetable = new Topping(new Rectangle(16, 16, 16, 16));
            toppings = new List<Topping>();
            toppings.Add(cheese);
            toppings.Add(pepperoni);
            toppings.Add(mushroom);
            toppings.Add(vegetable);
        }

        public readonly Rectangle textureRect;

        private Topping(Rectangle rect) {
            this.textureRect = rect;
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
