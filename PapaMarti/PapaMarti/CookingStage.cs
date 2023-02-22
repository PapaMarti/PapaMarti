using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PapaMarti
{
    public enum CookStage
    {
        Cutting,
        Toppings,
        Cooking
    }
    public abstract class CookingStage
    {
        protected readonly Pizza type;

        public CookingStage(Pizza type)
        {
            this.type = type;
        }

        public abstract void draw(SpriteBatch spriteBatch);
        public abstract void update(GameTime time);
        public abstract bool isDone();
        public abstract double getAccuracy();
        public abstract CookStage getStage();
    }
}
