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
    class Button
    {
        Texture2D texture;
        public Rectangle rectangle;

        public string title;

        public SpriteFont font;

        bool pressed;

        MouseState previous;

        public Button(Texture2D texture, Rectangle rectangle, string title, SpriteFont font)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            this.title = title;
            this.font = font;
            pressed = false;
            previous = Mouse.GetState();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.LightGray);
            spriteBatch.DrawString(font, title, new Vector2(rectangle.X + rectangle.Width / 2 - font.MeasureString(title).X / 2, rectangle.Y + rectangle.Height / 2 - font.MeasureString(title).Y / 2), Color.Black);
        }

        public void update()
        {
            MouseState mouse = Mouse.GetState();
            if(rectangle.Contains(new Point(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed && previous.LeftButton == ButtonState.Released)
            {
                pressed = true;
            }
            else
            {
                pressed = false;
            }
            previous = mouse;
        }

        public bool wasPressed()
        {
            return pressed;
        }
    }
}
