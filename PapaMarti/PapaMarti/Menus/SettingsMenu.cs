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

    public class SettingsMenu: Submenu
    {
        Slider volumeBar;
        Button apply;

        public SettingsMenu(ContentManager content, SaveManager saveData): base(content, "Settings")
        {
            volumeBar = new Slider(content, new Point(Game1.screenRect.Width / 2, Game1.screenRect.Height / 2), 500, 100, 50);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            volumeBar.draw(spriteBatch);
        }
        public override void update()
        {
            base.update();
            volumeBar.update();
        }
    }
}
