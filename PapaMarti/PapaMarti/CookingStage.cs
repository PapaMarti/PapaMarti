using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PapaMarti
{
    public abstract class CookingStage
    {
        public abstract void draw(SpriteBatch spriteBatch);
        public abstract void update(GameTime time);
        public abstract bool isDone();
        public abstract double getAccuracy();
    }
}

