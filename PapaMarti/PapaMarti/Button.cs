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
    public enum ButtonType
    {
        Text,
        Image
    }

    public class Button
    {
        Texture2D texture;
        public Rectangle rectangle;
        bool pressed;
        bool hover;
        MouseState previous;
        ButtonType type;

        public string title;
        public SpriteFont font;

        public Button(Texture2D texture, Rectangle rectangle, string title, SpriteFont font, ButtonType type)
        {
            this.type = type;
            this.texture = texture;
            this.rectangle = rectangle;
            this.title = title;
            this.font = font;
            pressed = false;
            hover = false;
            previous = Mouse.GetState();
        }
        public Button(Texture2D texture, Rectangle rectangle): this(texture, rectangle, String.Empty, null, ButtonType.Image) { }
        public Button(Texture2D texture, Rectangle rectangle, string title, SpriteFont font): this(texture, rectangle, title, font, ButtonType.Text) { }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.LightGray);
            if(type == ButtonType.Text)
                spriteBatch.DrawString(font, title, new Vector2(rectangle.X + rectangle.Width / 2 - font.MeasureString(title).X / 2, rectangle.Y + rectangle.Height / 2 - font.MeasureString(title).Y / 2), Color.Black);

            if (hover)
            {
                spriteBatch.Draw(texture, rectangle, Game1.shaded);
            }
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

            if(rectangle.Contains(new Point(mouse.X, mouse.Y)))
            {
                hover = true;
            }
            else
            {
                hover = false;
            }

            previous = mouse;
        }

        public bool wasPressed()
        {
            return pressed;
        }

        public bool beingPressed()
        {
            MouseState mouse = Mouse.GetState();
            return rectangle.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed;
        }
    }
}
