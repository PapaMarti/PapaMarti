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
        public ContentManager content;
        public Texture2D whitePixel;
        public Button backButton;
        public Submenu(ContentManager content, string name)
        {
            this.content = content;
            this.name = name;
            whitePixel = content.Load<Texture2D>("whitePixel");
            backButton = new Button(whitePixel, new Rectangle(10, 10, 150, 50), "Back", content.Load<SpriteFont>("ButtonTitle"));
        }

        public virtual void update()
        {
            backButton.update();
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(whitePixel, Game1.screenRect, Game1.shaded);
            backButton.draw(spriteBatch);
        }

        public virtual bool isDone()
        {
            if(backButton.wasPressed())
                Console.WriteLine(backButton.wasPressed());
            return Keyboard.GetState().IsKeyDown(Keys.Escape) || backButton.wasPressed();
        }
    }
}
