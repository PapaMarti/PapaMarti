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
using System.Text;

namespace PapaMarti
{
    public abstract class Task {
        public Room room;

        public abstract void update();
        public abstract void draw(SpriteBatch spriteBatch);
        public abstract bool isDone();
    }
}
