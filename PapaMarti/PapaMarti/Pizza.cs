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

public enum Topping
{
    tomatoSauce,
    cheese,
    pepperoni,
    sausage,
    olives,
    onions,
    chicken,
    tomatoes,
    jalapenos,
    mushrooms
}

namespace PapaMarti
{
    public class Pizza
    {
        List<Rectangle> outline;
        List<Topping> toppings;
        int cookTime;
        int quality;

        /// <summary>
        /// Creates a pizza that holds its specific values.
        /// </summary>
        /// <param name="cookTime">The amount of time it takes to cook this pizza.</param>
        /// <param name="toppings">The types of toppings that this pizza will have.</param>
        /// <param name="outline">List of Rectangles outlining where the pizza is supposed to be cut</param>
        public Pizza(List<Rectangle> outline, List<Topping> toppings, int cookTime)
        {
            this.outline = outline;
            this.toppings = toppings;
            this.cookTime = cookTime;
            this.quality = 0;
        }


    }
}
