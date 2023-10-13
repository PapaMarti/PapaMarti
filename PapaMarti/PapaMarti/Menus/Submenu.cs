using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PapaMarti
{
    public abstract class Submenu
    {
        public string name;
        ContentManager content;
        public Submenu(ContentManager content, string name)
        {
            this.content = content;
            this.name = name;
        }

        abstract public void update();

        abstract public void draw(SpriteBatch spriteBatch);

        abstract public bool isDone();
    }
}
