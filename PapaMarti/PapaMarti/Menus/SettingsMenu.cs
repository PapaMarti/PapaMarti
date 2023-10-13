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
        Button backButton;

        public SettingsMenu(ContentManager content): base(content, "Settings")
        {
            volumeBar = new Slider(content, new Point(Game1.screenRect.Width / 2, Game1.screenRect.Height / 2), 500, 100, 50);
            backButton = new Button(content.Load<Texture2D>("whitePixel"), new Rectangle(10, 10, 150, 50), "Back", content.Load<SpriteFont>("ButtonTitle"));
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            backButton.draw(spriteBatch);
            volumeBar.draw(spriteBatch);
        }
        public override void update()
        {
            backButton.update();
            volumeBar.update();
        }
        public override bool isDone()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape) || backButton.wasPressed();
        }
    }
}
