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

namespace PapaMarti
{
    class ToppingScreen : CookingStage
    {
        List<ToppingContainer> toppingContainers;
        Texture2D spriteSheet;

        /// <summary>
        /// Creates a new topping screen to complete the topping stage of creating the pizza
        /// </summary>
        public ToppingScreen(Pizza type, Texture2D spriteSheet, Rectangle screen) : base(type)
        {
            toppingContainers = new List<ToppingContainer>();
            this.spriteSheet = spriteSheet;

            Array toppings = Enum.GetNames(typeof(Topping));
            for (int i = 0; i < toppings.Length; i++)
            {
                if (i < toppings.Length / 2)
                {
                    toppingContainers.Add(new ToppingContainer((Topping)i, new Rectangle(100, i * 100 + 500, 100, 50)));
                }
                if (i >= toppings.Length / 2)
                {
                    toppingContainers.Add(new ToppingContainer((Topping)i, new Rectangle(screen.Right - 200, i * 100, 100, 50)));
                }
            }
        }

        override
        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < toppingContainers.Count; i++)
            {
                toppingContainers[i].draw(spriteBatch, spriteSheet, new Rectangle(0, i * 10, 10, 10));
            }
        }

        override
        public void update(GameTime time)
        {

        }

        override
        public bool isDone()
        {
            return true;
        }

        override
        public double getAccuracy()
        {
            return 0;
        }

        override
        public CookStage getStage()
        {
            return CookStage.Toppings;
        }
    }
}
