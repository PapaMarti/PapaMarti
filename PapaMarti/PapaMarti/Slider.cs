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
    public class Slider
    {
        public static int BAR_HEIGHT = 20;
        public static int SLIDE_HEIGHT = 50;

        ContentManager content;
        Rectangle bar;
        Button slide;
        Texture2D white;
        double value;
        double scale;
        bool slideWasPressed;

        public Slider(ContentManager content, Point center, int length, double scale, double startingValue)
        {
            this.content = content;
            white = content.Load<Texture2D>("whitePixel");
            bar = new Rectangle(center.X - length / 2, center.Y - BAR_HEIGHT / 2, length, BAR_HEIGHT);
            value = startingValue;
            this.scale = scale;
            slide = new Button(white, new Rectangle((int)(bar.X - BAR_HEIGHT / 2 + value / scale * bar.Width), center.Y - SLIDE_HEIGHT / 2, BAR_HEIGHT, SLIDE_HEIGHT));
            slideWasPressed = false;
        }
        public Slider(ContentManager content, Point center, int length, int scale): this(content, center, length, scale, 0) { }

        public double getValue()
        {
            return value;
        }

        public void update()
        {
            MouseState mouse = Mouse.GetState();
            slide.update();
            if(mouse.LeftButton == ButtonState.Pressed)
            {
                if (slideWasPressed)
                {
                    slide.rectangle.X = mouse.X - BAR_HEIGHT / 2;
                    if (slide.rectangle.X < bar.X - BAR_HEIGHT / 2)
                    {
                        slide.rectangle.X = bar.X - BAR_HEIGHT / 2;
                    }
                    else if (slide.rectangle.X > bar.X + bar.Width - BAR_HEIGHT / 2)
                    {
                        slide.rectangle.X = bar.X + bar.Width - BAR_HEIGHT / 2;
                    }
                    value = (double)(slide.rectangle.X - bar.X - BAR_HEIGHT / 2) / bar.X * scale;
                }
                else if(slide.wasPressed())
                {
                    slideWasPressed = true;
                }
            }
            else if (slideWasPressed)
            {
                slideWasPressed = false;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(white, bar, Color.Gray);
            slide.draw(spriteBatch);
        }
    }
}
