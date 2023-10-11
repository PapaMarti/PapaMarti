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
    public class TextCard
    {
        string text; //the contents of the text card
        List<string> lines; //the text after it has been broken up into all the lines needed
        string name; // if the text card is meant to represent a person speaking, this is that persons name, or the name that will appear on the screen
        ContentManager content;
        Rectangle textBox;
        bool hasName; //some text cards may not have a name (like tutorial cards)
        bool done; // if someone has clicked the card this will be true
        Texture2D cardTexture;
        SpriteFont nameFont;
        SpriteFont font;
        MouseState previousMouse;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">The text to be displayed on the card</param>
        /// <param name="name">The name of the person speaking. If there is none, put an empty string</param>
        public TextCard(ContentManager content, string text, string name)
        {
            this.text = text;
            this.name = name;
            if (name.Length > 0)
            {
                hasName = true;
                cardTexture = content.Load<Texture2D>("whitePixel"); //if there are different textures for whether there is a name
                nameFont = content.Load<SpriteFont>("text01"); 
            }
            else
            {
                hasName = false;
                cardTexture = content.Load<Texture2D>("whitePixel");
            }

            font = content.Load<SpriteFont>("text01");
            this.content = content;
            int boxHeight = (int)(Game1.screenRect.Height * 0.4);
            textBox = new Rectangle(0, Game1.screenRect.Height - boxHeight, Game1.screenRect.Width, boxHeight);

            lines = new List<string>();
            string[] words = text.Split(' ');
            string line = "";
            for(int i = 0; i < words.Length; i++)
            {
                if(font.MeasureString(line + words[i]).X > textBox.Width - 60)
                {
                    lines.Add(line);
                    line = words[i] + " ";
                }
                else
                {
                    line += words[i] + " ";
                    if (i == words.Length - 1)
                        lines.Add(line);
                }
            }

            previousMouse = Mouse.GetState();
        }

        public void update()
        {
            MouseState mouse = Mouse.GetState();
            if(mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed && mouse.Y > textBox.Y)
            {
                done = true;
            }
            previousMouse = mouse;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cardTexture, textBox, Color.White);
            Vector2 textLocation = new Vector2(30, textBox.Y + 30);
            int spacing = 5;
            if (hasName)
            {
                spriteBatch.DrawString(nameFont, name, textLocation, Color.Black);
                textLocation.Y += nameFont.MeasureString(name).Y + spacing * 2;
            }
            for(int i = 0; i < lines.Count; i++)
            {
                spriteBatch.DrawString(font, lines[i], textLocation, Color.Black);
                textLocation.Y += font.MeasureString(lines[i]).Y + spacing;
            }
        }

        public bool isDone()
        {
            return done;
        }
    }
}
