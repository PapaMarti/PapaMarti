using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PapaMarti
{
    class Toppings
    {
        string name;
        Rectangle panel;
        Texture2D texture;

        public Toppings(string name, Texture2D texture, Rectangle panel)
        {
            this.texture = texture;
            this.name = name;
            this.panel = panel;
        }
    }
}
