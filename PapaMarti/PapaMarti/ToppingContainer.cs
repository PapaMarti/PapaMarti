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
    class ToppingContainer
    {
        Topping type;
        Rectangle location;

        public ToppingContainer(Topping type, Rectangle location)
        {
            this.type = type;
            this.location = location;
        }

        public void draw(SpriteBatch spritebatch, Texture2D image, Rectangle sourceRectangle)
        {
            spritebatch.Draw(image, location, sourceRectangle, Color.White);
        }
    }
}
