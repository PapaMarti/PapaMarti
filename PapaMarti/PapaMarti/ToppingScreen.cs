using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    class ToppingScreen
    {
        Pizza pizza;

        /// <summary>
        /// Creates a new topping screen to complete the topping stage of creating the pizza
        /// </summary>
        /// <param name="pizza">The pizza template being used.</param>
        public ToppingScreen(Pizza pizza)
        {
            this.pizza = pizza;
        }

        
    }
}
