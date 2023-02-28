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

public enum Toppings
{
    tomatoSauce,
    cheese,
    pepperoni,
    sausage,
    olives,
    onions,
    chicken,
    tomato,
    jalapeno
}

namespace PapaMarti
{
    public class Pizza
    {
        public PizzaShape shape;
        public List<Toppings> toppings;
        public int cookTime;

        /// <summary>
        /// Creates a pizza that holds its specific values.
        /// </summary>
        /// <param name="cookTime">The amount of time it takes to cook this pizza.</param>
        /// <param name="toppings">The types of toppings that this pizza will have.</param>
        /// <param name="shape">Enum for what shape of the pizza is for the Cutting Stage.</param>
        public Pizza(PizzaShape shape, List<Toppings> toppings, int cookTime)
        {
            this.shape = shape;
            this.toppings = toppings;
            this.cookTime = cookTime;
        }
    }
}
